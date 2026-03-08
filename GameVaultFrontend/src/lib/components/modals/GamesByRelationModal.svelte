<script>
  /**
   * @component GamesByRelationModal
   * Универсальное модальное окно для отображения списка игр,
   * отфильтрованных по жанру или издателю.
   */
  import BaseModal from './BaseModal.svelte';
  import { tableStore } from '$lib/stores/tableStore.svelte';
  import { gamesApi } from '$lib/api/games';
  import { onMount } from 'svelte';
  import { mapError } from '$lib/utils/errorMapper';
  import { fade, fly } from 'svelte/transition';

  const item = tableStore.currentItem;
  const type = tableStore.currentTableId; // 'genres' или 'publishers'
  
  let games = $state([]);
  let loading = $state(true);
  let error = $state(null);

  const title = $derived(
    type === 'genres' 
      ? `Игры жанра: ${item?.name}` 
      : `Игры издателя: ${item?.company}`
  );

  onMount(async () => {
    loading = true;
    try {
      if (type === 'genres') {
        games = await gamesApi.getSliceByGenre(0, item.name);
      } else {
        games = await gamesApi.getSliceByPublisher(0, item.company);
      }
    } catch (err) {
      error = mapError(err);
    } finally {
      loading = false;
    }
  });

  function formatDate(date) {
    if (!date) return '-';
    return new Date(date).toLocaleDateString('ru-RU', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  }
</script>

<BaseModal {title} onclose={() => tableStore.closeModal()}>
  <div class="games-relation-view">
    {#if loading}
      <div class="loader">
        <div class="spinner small"></div>
        <span>Загрузка игр...</span>
      </div>
    {:else if error}
      <div class="error-msg">{error}</div>
    {:else}
      {#if games.length === 0}
        <div class="empty-state" in:fade>
          <div class="empty-icon">🎮</div>
          <p>Игры не найдены</p>
        </div>
      {:else}
        <div class="games-list">
          {#each games as game (game.title)}
            <div class="game-card" in:fly={{ y: 20, duration: 300 }}>
              <div class="game-icon">🎲</div>
              <div class="game-info">
                <span class="game-title">{game.title}</span>
                <span class="game-meta">{game.company} • {game.weight} ГБ</span>
                <span class="game-date">Релиз: {formatDate(game.release_date)}</span>
              </div>
            </div>
          {/each}
        </div>
      {/if}
    {/if}
  </div>
</BaseModal>

<style>
  .games-relation-view {
    width: 100%;
    max-width: 500px;
    min-width: 320px;
    max-height: 50vh;
    overflow-y: auto;
    padding-right: 0.5rem;
  }

  .games-relation-view::-webkit-scrollbar {
    width: 6px;
  }
  .games-relation-view::-webkit-scrollbar-thumb {
    background: #e2e8f0;
    border-radius: 10px;
  }

  .loader {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    gap: 1rem;
    padding: 3rem;
    color: #64748b;
  }

  .games-list {
    display: flex;
    flex-direction: column;
    gap: 0.75rem;
  }

  .game-card {
    display: flex;
    align-items: center;
    gap: 1rem;
    padding: 0.875rem;
    background: white;
    border: 1px solid #e2e8f0;
    border-radius: 14px;
    transition: all 0.2s;
  }

  .game-card:hover {
    border-color: #6366f1;
    background: #fdfdff;
    box-shadow: 0 4px 12px rgba(99, 102, 241, 0.05);
  }

  .game-icon {
    width: 42px;
    height: 42px;
    background: linear-gradient(135deg, #f0f9ff, #e0f2fe);
    border: 1px solid #bae6fd;
    color: #0369a1;
    border-radius: 10px;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 1.25rem;
    flex-shrink: 0;
  }

  .game-info {
    flex: 1;
    display: flex;
    flex-direction: column;
    gap: 0.125rem;
  }

  .game-title {
    font-weight: 700;
    color: #1e293b;
    font-size: 0.95rem;
  }

  .game-meta {
    font-size: 0.8rem;
    color: #64748b;
  }

  .game-date {
    font-size: 0.7rem;
    color: #94a3b8;
    margin-top: 0.125rem;
  }

  .error-msg {
    background: #fef2f2;
    color: #ef4444;
    padding: 0.75rem 1rem;
    border-radius: 12px;
    font-size: 0.9rem;
    border: 1px solid #fee2e2;
  }

  .empty-state {
    text-align: center;
    padding: 3rem 1rem;
    color: #94a3b8;
  }

  .empty-icon {
    font-size: 2.5rem;
    margin-bottom: 0.75rem;
    opacity: 0.5;
  }

  .spinner.small {
    width: 24px;
    height: 24px;
    border: 3px solid #f3f3f3;
    border-top: 3px solid #6366f1;
    border-radius: 50%;
    animation: spin 1s linear infinite;
  }

  @keyframes spin {
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
  }
</style>
