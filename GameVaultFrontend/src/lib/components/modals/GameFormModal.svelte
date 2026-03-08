<script>
  import BaseModal from './BaseModal.svelte';
  import { tableStore } from '$lib/stores/tableStore.svelte';
  import { mapError } from '$lib/utils/errorMapper';
  import { validateForm, normalize } from '$lib/utils/validation';

  let isEdit = $derived(!!tableStore.currentItem);
  
  let title = $state(tableStore.currentItem?.title || '');
  let company = $state(tableStore.currentItem?.company || '');
  let weight = $state(tableStore.currentItem?.weight || 0);
  let releaseDate = $state(tableStore.currentItem?.release_date ? new Date(tableStore.currentItem.release_date).toISOString().split('T')[0] : '');
  
  let loading = $state(false);
  let error = $state(null);

  async function handleSubmit(e) {
    e.preventDefault();
    loading = true;
    error = null;

    // Client side validation
    const validationError = validateForm(
      { title, company, weight, releaseDate }, 
      { title: 'text', company: 'text', weight: 'positiveNumber', releaseDate: 'date' }
    );
    
    if (validationError) {
      error = validationError;
      loading = false;
      return;
    }

    try {
      const payload = { 
        title: title,
        company: company,
        weight: parseFloat(weight),
        releaseDate: new Date(releaseDate).toISOString()
      };

      if (isEdit) {
        // Update uses Title as fixed ID in this case, similar to Publishers
        await tableStore.updateGame({
          title: title,
          newCompany: company,
          newWeight: parseFloat(weight),
          newReleaseDate: payload.releaseDate
        });
      } else {
        await tableStore.createGame(payload);
      }
    } catch (err) {
      error = mapError(err);
    } finally {
      loading = false;
    }
  }
</script>

<BaseModal 
  title={isEdit ? 'Редактировать игру' : 'Добавить игру'} 
  onclose={() => tableStore.closeModal()}
>
  <form onsubmit={handleSubmit} class="form">
    <div class="field">
      <label for="title">Название игры</label>
      <input 
        id="title"
        type="text" 
        bind:value={title} 
        required 
        placeholder="Например: The Witcher 3"
        disabled={loading || isEdit}
      />
      {#if isEdit}
        <p class="hint">Название игры нельзя изменить</p>
      {/if}
    </div>

    <div class="field">
      <label for="company">Издатель</label>
      <input 
        id="company"
        type="text" 
        bind:value={company} 
        required 
        placeholder="Например: CD Projekt"
        disabled={loading}
      />
    </div>

    <div class="grid-2">
      <div class="field">
        <label for="weight">Вес (ГБ)</label>
        <input 
          id="weight"
          type="number" 
          step="0.1"
          bind:value={weight} 
          required 
          disabled={loading}
        />
      </div>

      <div class="field">
        <label for="releaseDate">Дата релиза</label>
        <input 
          id="releaseDate"
          type="date" 
          bind:value={releaseDate} 
          required 
          disabled={loading}
        />
      </div>
    </div>

    {#if error}
      <div class="error">{error}</div>
    {/if}

    <div class="actions">
      <button type="button" class="btn-secondary" onclick={() => tableStore.closeModal()}>Отмена</button>
      <button type="submit" class="btn-primary" disabled={loading}>
        {loading ? 'Сохранение...' : (isEdit ? 'Сохранить' : 'Добавить')}
      </button>
    </div>
  </form>
</BaseModal>

<style>
  .form {
    display: flex;
    flex-direction: column;
    gap: 1.5rem;
    min-width: 400px;
  }

  .grid-2 {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 1rem;
  }

  .field {
    display: flex;
    flex-direction: column;
    gap: 0.5rem;
  }

  label {
    font-weight: 600;
    font-size: 0.875rem;
    color: #4b5563;
  }

  input {
    padding: 0.875rem 1rem;
    border: 1px solid #e2e8f0;
    border-radius: 12px;
    font-size: 1rem;
    color: #1e293b;
    transition: all 0.2s;
    background: #f8fafc;
  }

  input:focus {
    outline: none;
    border-color: #6366f1;
    background: white;
    box-shadow: 0 0 0 4px rgba(99, 102, 241, 0.1);
  }

  input:disabled {
    background: #f1f5f9;
    color: #64748b;
    cursor: not-allowed;
  }

  .hint {
    font-size: 0.75rem;
    color: #64748b;
    margin: 0;
  }

  .actions {
    display: flex;
    justify-content: flex-end;
    gap: 1rem;
    margin-top: 1.5rem;
    padding-top: 1.5rem;
    border-top: 1px solid #f1f5f9;
  }

  button {
    padding: 0.8rem 1.5rem;
    border-radius: 12px;
    font-weight: 700;
    cursor: pointer;
    transition: all 0.2s cubic-bezier(0.4, 0, 0.2, 1);
  }

  .btn-primary {
    background: #6366f1;
    color: white;
    border: none;
    box-shadow: 0 4px 12px rgba(99, 102, 241, 0.25);
  }

  .btn-primary:hover:not(:disabled) {
    background: #4f46e5;
    transform: translateY(-2px);
    box-shadow: 0 6px 16px rgba(99, 102, 241, 0.35);
  }

  .btn-secondary {
    background: #f1f5f9;
    color: #475569;
    border: 1px solid #e2e8f0;
  }

  .error {
    color: #ef4444;
    font-size: 0.875rem;
    background: #fef2f2;
    padding: 1rem;
    border-radius: 12px;
    border: 1px solid #fee2e2;
    display: flex;
    align-items: center;
    gap: 0.5rem;
  }

  .error::before {
    content: '⚠️';
  }
</style>
