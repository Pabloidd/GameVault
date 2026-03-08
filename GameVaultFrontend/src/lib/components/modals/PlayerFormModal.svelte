<script>
  /**
   * @component PlayerFormModal
   * Модальное окно для создания и редактирования игроков.
   * Особенности:
   * - Строгая валидация Email и уровней.
   * - Ограничение даты регистрации (2000 - сегодня).
   * - Разное поведение для создания и обновления (блокировка никнейма).
   */
  import BaseModal from './BaseModal.svelte';
  import { tableStore } from '$lib/stores/tableStore.svelte';
  import { mapError } from '$lib/utils/errorMapper';

  // Определяем, режим это создания или обновления
  const isUpdate = $derived(!!tableStore.currentItem);
  const title = $derived(isUpdate ? 'Редактировать игрока' : 'Создать игрока');

  // Даты для ограничений в <input>
  let today = new Date().toISOString().split('T')[0];
  let now = new Date().toISOString().slice(0, 16);

  // Реактивный объект данных формы
  let formData = $state({
    nickname: tableStore.currentItem?.nickname || '',
    email: tableStore.currentItem?.email || '',
    level: tableStore.currentItem?.level || 0,
    registrationDate: tableStore.currentItem?.registrationDate 
      ? new Date(tableStore.currentItem.registrationDate).toISOString().slice(0, 16)
      : now
  });

  let error = $state(null);
  let loading = $state(false);

  /**
   * Валидация Email через регулярное выражение.
   * Требует наличия домена (напр. .com, .ru).
   */
  function validateEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
  }

  /**
   * Обработка отправки формы.
   * Выполняет комплексную проверку перед вызовом API.
   */
  async function handleSubmit() {
    // 1. Валидация почты
    if (!validateEmail(formData.email)) {
      error = 'Пожалуйста, введите корректный Email (напр. user@example.com)';
      return;
    }

    // 2. Валидация диапазона дат
    const regDate = new Date(formData.registrationDate);
    const minDate = new Date('2000-01-01');
    const maxDate = new Date();

    if (regDate < minDate || regDate > maxDate) {
      error = 'Дата регистрации должна быть в диапазоне от 2000 года до сегодняшнего дня';
      return;
    }

    // 3. Валидация уровня (кап 999)
    if (formData.level > 999) {
      error = 'Уровень не может быть выше 999';
      return;
    }

    loading = true;
    error = null;
    try {
      if (isUpdate) {
        // При обновлении используем формат, ожидаемый Backend (UpdatePlayerRequest)
        await tableStore.updatePlayer({
          nickname: formData.nickname,
          newEmail: formData.email,
          newLevel: formData.level
        });
      } else {
        // При создании отправляем полный объект
        await tableStore.createPlayer({
          ...formData,
          registrationDate: new Date(formData.registrationDate).toISOString()
        });
      }
    } catch (err) {
      // Маппим ошибку сервера в понятный текст
      error = mapError(err);
    } finally {
      loading = false;
    }
  }
</script>

<BaseModal {title} onclose={() => tableStore.closeModal()}>
  <form onsubmit={(e) => { e.preventDefault(); handleSubmit(); }} class="player-form">
    {#if error}
      <div class="error-banner">{error}</div>
    {/if}

    <div class="form-group">
      <label for="nickname">Никнейм</label>
      <input 
        type="text" 
        id="nickname" 
        bind:value={formData.nickname} 
        placeholder="Напр. DragonSlayer" 
        disabled={isUpdate}
        required
      />
      {#if isUpdate}
        <small class="hint">Никнейм нельзя изменить</small>
      {/if}
    </div>

    <div class="form-group">
      <label for="email">Email</label>
      <input 
        type="email" 
        id="email" 
        bind:value={formData.email} 
        placeholder="example@mail.com" 
        required
      />
    </div>

    <div class="form-row">
      <div class="form-group">
        <label for="level">Уровень</label>
        <input 
          type="number" 
          id="level" 
          bind:value={formData.level} 
          min="0"
          max="999"
          required
        />
      </div>

      <div class="form-group">
        <label for="registrationDate">Дата регистрации</label>
        <input 
          type="datetime-local" 
          id="registrationDate" 
          bind:value={formData.registrationDate} 
          min="2000-01-01T00:00"
          max={now}
          disabled={isUpdate}
          required
        />
      </div>
    </div>

    <div class="form-actions">
      <button type="button" class="cancel-btn" onclick={() => tableStore.closeModal()}>Отмена</button>
      <button type="submit" class="submit-btn" disabled={loading}>
        {#if loading}
          <div class="spinner small white"></div>
        {:else}
          {isUpdate ? 'Сохранить изменения' : 'Создать игрока'}
        {/if}
      </button>
    </div>
  </form>
</BaseModal>

<style>
  .player-form {
    display: flex;
    flex-direction: column;
    gap: 1.5rem;
    min-width: 400px;
  }

  .form-row {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 1.5rem;
  }

  .form-group {
    display: flex;
    flex-direction: column;
    gap: 0.5rem;
  }

  label {
    font-weight: 600;
    color: #475569;
    font-size: 0.9rem;
  }

  input {
    padding: 0.75rem 1rem;
    border: 1.5px solid #e2e8f0;
    border-radius: 10px;
    font-size: 1rem;
    transition: all 0.2s;
  }

  input:focus {
    outline: none;
    border-color: #6366f1;
    box-shadow: 0 0 0 4px rgba(99, 102, 241, 0.1);
  }

  input:disabled {
    background: #f8fafc;
    cursor: not-allowed;
  }

  .hint {
    color: #94a3b8;
    font-size: 0.8rem;
    margin-top: 0.25rem;
  }

  .error-banner {
    background: #fef2f2;
    color: #ef4444;
    padding: 1rem;
    border-radius: 10px;
    border: 1px solid #fee2e2;
    font-size: 0.9rem;
  }

  .form-actions {
    display: flex;
    justify-content: flex-end;
    gap: 1rem;
    margin-top: 1rem;
  }

  .cancel-btn {
    padding: 0.75rem 1.5rem;
    background: white;
    border: 1.5px solid #e2e8f0;
    border-radius: 10px;
    color: #64748b;
    font-weight: 600;
    cursor: pointer;
    transition: all 0.2s;
  }

  .cancel-btn:hover {
    background: #f8fafc;
    border-color: #cbd5e1;
  }

  .submit-btn {
    padding: 0.75rem 1.5rem;
    background: #6366f1;
    border: none;
    border-radius: 10px;
    color: white;
    font-weight: 600;
    cursor: pointer;
    transition: all 0.2s;
    display: flex;
    align-items: center;
    justify-content: center;
    min-width: 160px;
  }

  .submit-btn:hover {
    background: #4f46e5;
    transform: translateY(-1px);
    box-shadow: 0 4px 12px rgba(99, 102, 241, 0.2);
  }

  .submit-btn:disabled {
    opacity: 0.7;
    cursor: not-allowed;
    transform: none;
  }

  .spinner.small {
    width: 20px;
    height: 20px;
    border: 2px solid rgba(255, 255, 255, 0.3);
    border-top-color: white;
    border-radius: 50%;
    animation: spin 1s linear infinite;
  }

  @keyframes spin {
    to { transform: rotate(360deg); }
  }
</style>
