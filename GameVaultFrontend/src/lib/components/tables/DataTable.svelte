<script>
  /**
   * @component DataTable
   * Универсальный компонент для отображения данных в виде таблицы.
   * Основные функции:
   * 1. Отображение данных из глобального tableStore.
   * 2. Динамическое построение заголовков и строк на основе конфигурации (constants.js).
   * 3. Реализация бесконечной прокрутки через Intersection Observer.
   * 4. Вызов модальных окон для редактирования, удаления и спец. действий.
   */
  import { tableStore } from '$lib/stores/tableStore.svelte';
  import { onMount } from 'svelte';

  // Вычисляемое свойство: список колонок для текущей таблицы
  const columns = $derived(tableStore.currentTable?.columns || []);
  
  // Элемент-маяк (sentinel) в конце таблицы для срабатывания бесконечной прокрутки
  let sentinel = $state(null);
  
  // Логирование состояния (полезно при разработке для отслеживания реактивных изменений)
  $effect(() => {
    console.log('DataTable state:', {
      tableId: tableStore.currentTableId,
      dataLength: tableStore.data.length,
      columnsCount: columns.length,
      loading: tableStore.loading
    });
  });

  /**
   * Эффект инициализации Intersection Observer.
   * Срабатывает, когда элемент 'sentinel' появляется в области видимости.
   */
  $effect(() => {
    // Прокрутка доступна только для игр и игроков
    const isSlicable = ['games', 'players'].includes(tableStore.currentTableId);
    
    if (sentinel && tableStore.hasMore && isSlicable) {
      const observer = new IntersectionObserver((entries) => {
        // Если маяк виден и мы сейчас не в процессе загрузки - запрашиваем следующую порцию
        if (entries[0].isIntersecting && !tableStore.loadingNext) {
          tableStore.fetchNextSlice();
        }
      }, { threshold: 0.1 });

      observer.observe(sentinel);
      
      // Cleanup: отключаем observer при уничтожении компонента или изменении условий
      return () => observer.disconnect();
    }
  });

  /**
   * Вспомогательная функция для форматирования значений ячеек.
   * @param value - Исходное значение из БД
   * @param key - Ключ поля (используется для определения типа данных, например даты)
   */
  function formatValue(value, key) {
    if (value === null || value === undefined) return '-';
    
    // Если в ключе есть намек на дату - форматируем в RU стандарт
    if (key.toLowerCase().includes('date') || key.toLowerCase().includes('time')) {
      try {
        return new Date(value).toLocaleDateString('ru-RU');
      } catch (e) {
        return value;
      }
    }
    return value;
  }
</script>

<div class="data-table-container">

  {#if tableStore.loading && tableStore.data.length === 0}
    <div class="table-loading">
      <div class="spinner"></div>
      <p>Загрузка данных...</p>
    </div>
  {:else if tableStore.error}
    <div class="table-error">
      <span class="error-icon">⚠️</span>
      <p>{tableStore.error}</p>
      <button onclick={() => tableStore.fetchData()}>Попробовать снова</button>
    </div>
  {:else}
    <div class="table-wrapper">
      <table class="data-table">
        <thead>
          <tr>
            {#each columns as col}
              <th>{col.label}</th>
            {/each}
            <th class="actions-header">Действия</th>
          </tr>
        </thead>
        <tbody>
          {#each tableStore.data as row}
            <tr>
              {#each columns as col}
                <td>{formatValue(row[col.key], col.key)}</td>
              {/each}
              <td class="actions-cell">
                {#if tableStore.currentTableId === 'games'}
                  <button class="action-btn genres" onclick={() => tableStore.openModal('gameGenres', row)} title="Управление жанрами">
                    <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M20.59 13.41l-7.17 7.17a2 2 0 0 1-2.83 0L2 12V2h10l8.59 8.59a2 2 0 0 1 0 2.82z"></path><line x1="7" y1="7" x2="7.01" y2="7"></line></svg>
                  </button>
                  <button class="action-btn players" onclick={() => tableStore.openModal('gamePlayers', row)} title="Просмотреть игроков">
                    <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"></path><circle cx="9" cy="7" r="4"></circle><path d="M23 21v-2a4 4 0 0 0-3-3.87"></path><path d="M16 3.13a4 4 0 0 1 0 7.75"></path></svg>
                  </button>
                {/if}
                {#if tableStore.currentTableId === 'players'}
                  <button class="action-btn games" onclick={() => tableStore.openModal('playerGames', row)} title="Просмотреть игры">
                    <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><rect x="2" y="2" width="20" height="20" rx="5" ry="5"></rect><path d="M6 12h12"></path><path d="M12 6v12"></path></svg>
                  </button>
                {/if}
                <button class="action-btn edit" onclick={() => tableStore.openModal('update', row)} title="Редактировать">
                  <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M17 3a2.828 2.828 0 1 1 4 4L7.5 20.5 2 22l1.5-5.5L17 3z"></path></svg>
                </button>
                <button class="action-btn delete" onclick={() => tableStore.openModal('delete', row)} title="Удалить">
                  <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><polyline points="3 6 5 6 21 6"></polyline><path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"></path><line x1="10" y1="11" x2="10" y2="17"></line><line x1="14" y1="11" x2="14" y2="17"></line></svg>
                </button>
              </td>
            </tr>
          {/each}
        </tbody>
      </table>

      {#if tableStore.hasMore && ['games', 'players'].includes(tableStore.currentTableId)}
        <div bind:this={sentinel} class="sentinel">
          {#if tableStore.loadingNext}
            <div class="next-slice-loading">
              <div class="spinner small"></div>
              <span>Загрузка еще...</span>
            </div>
          {/if}
        </div>
      {/if}
      
      {#if tableStore.data.length === 0 && !tableStore.loading}
        <div class="empty-state">
          <p>Нет данных для отображения</p>
        </div>
      {/if}
    </div>
  {/if}
</div>

<style>
  .data-table-container {
    background: white;
    border-radius: 16px;
    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.05);
    overflow: hidden;
    margin-top: 2rem;
    border: 1px solid #f1f5f9;
  }

  .table-wrapper {
    overflow-x: auto;
  }

  .data-table {
    width: 100%;
    border-collapse: collapse;
    text-align: left;
  }

  th {
    background: #f8fafc;
    padding: 1.25rem 1.5rem;
    font-weight: 700;
    color: #64748b;
    font-size: 0.75rem;
    text-transform: uppercase;
    letter-spacing: 0.1em;
    border-bottom: 2px solid #f1f5f9;
  }

  td {
    padding: 1.25rem 1.5rem;
    color: #334155;
    font-size: 0.95rem;
    border-bottom: 1px solid #f1f5f9;
    transition: background-color 0.2s;
  }

  tr:last-child td {
    border-bottom: none;
  }

  tr:hover td {
    background-color: #f8fafc;
  }

  .actions-header {
    text-align: right;
  }

  .actions-cell {
    text-align: right;
    display: flex;
    justify-content: flex-end;
    gap: 0.75rem;
  }

  .action-btn {
    background: white;
    border: 1px solid #e2e8f0;
    border-radius: 10px;
    padding: 0;
    color: #64748b;
    transition: all 0.2s cubic-bezier(0.4, 0, 0.2, 1);
    display: flex;
    align-items: center;
    justify-content: center;
    width: 38px;
    height: 38px;
    cursor: pointer;
  }

  .action-btn:hover {
    transform: translateY(-2px);
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
  }

  .action-btn.edit:hover {
    color: #6366f1;
    border-color: #6366f1;
    background: #f5f3ff;
  }

  .action-btn.delete:hover {
    color: #ef4444;
    border-color: #ef4444;
    background: #fef2f2;
  }

  .action-btn.genres:hover {
    color: #10b981;
    border-color: #10b981;
    background: #ecfdf5;
  }

  .action-btn.players:hover {
    color: #f59e0b;
    border-color: #f59e0b;
    background: #fffbeb;
  }

  .spinner {
    width: 40px;
    height: 40px;
    border: 3px solid #f3f3f3;
    border-top: 3px solid #6366f1;
    border-radius: 50%;
    animation: spin 1s linear infinite;
    margin: 0 auto 1rem;
  }

  @keyframes spin {
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
  }

  .error-icon {
    font-size: 2rem;
    display: block;
    margin-bottom: 1rem;
  }

  button {
    margin-top: 1rem;
    padding: 0.5rem 1.5rem;
    background: #6366f1;
    color: white;
    border: none;
    border-radius: 8px;
    font-weight: 600;
  }

  .sentinel {
    height: 80px;
    display: flex;
    align-items: center;
    justify-content: center;
    border-top: 1px solid #f1f5f9;
    background: #fcfcfd;
  }

  .next-slice-loading {
    display: flex;
    align-items: center;
    gap: 0.75rem;
    color: #64748b;
    font-size: 0.875rem;
    font-weight: 500;
  }

  .spinner.small {
    width: 20px;
    height: 20px;
    border-width: 2px;
    margin: 0;
  }

  .empty-state {
    padding: 4rem 2rem;
    text-align: center;
    color: #64748b;
  }
</style>
