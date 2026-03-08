<script>
  import BaseModal from './BaseModal.svelte';
  import { tableStore } from '$lib/stores/tableStore.svelte';
  import { mapError } from '$lib/utils/errorMapper';

  let loading = $state(false);
  let error = $state(null);

  async function handleDelete() {
    loading = true;
    error = null;
    try {
      await tableStore.delete(tableStore.currentItem);
    } catch (err) {
      error = mapError(err);
    } finally {
      loading = false;
    }
  }
</script>

<BaseModal title="Подтверждение удаления" onclose={() => tableStore.closeModal()}>
  <div class="confirmation">
    <div class="warning-icon">
      <svg xmlns="http://www.w3.org/2000/svg" width="48" height="48" viewBox="0 0 24 24" fill="none" stroke="#ef4444" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M10.29 3.86L1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z"></path><line x1="12" y1="9" x2="12" y2="13"></line><line x1="12" y1="17" x2="12.01" y2="17"></line></svg>
    </div>
    <p>Вы уверены, что хотите удалить <strong>{tableStore.currentItem?.name || tableStore.currentItem?.company}</strong>?</p>
    <p class="warning-note">Это действие невозможно выполнить, если запись используется в других таблицах.</p>
    
    {#if error}
      <p class="error">{error}</p>
    {/if}

    <div class="actions">
      <button class="btn-secondary" onclick={() => tableStore.closeModal()} disabled={loading}>
        Отмена
      </button>
      <button class="btn-danger" onclick={handleDelete} disabled={loading}>
        {loading ? 'Удаление...' : 'Да, удалить'}
      </button>
    </div>
  </div>
</BaseModal>

<style>
  .confirmation {
    text-align: center;
  }

  .warning-icon {
    font-size: 3rem;
    margin-bottom: 1rem;
  }

  p {
    margin: 0.5rem 0;
    color: #1f2937;
  }

  .warning-note {
    font-size: 0.875rem;
    color: #6b7280;
    margin-bottom: 2rem;
  }

  .actions {
    display: flex;
    justify-content: center;
    gap: 1rem;
  }

  button {
    padding: 0.75rem 1.5rem;
    border-radius: 10px;
    font-weight: 600;
    cursor: pointer;
    transition: all 0.2s;
  }

  .btn-danger {
    background: #ef4444;
    color: white;
    border: none;
  }

  .btn-danger:hover:not(:disabled) {
    background: #dc2626;
  }

  .btn-secondary {
    background: #f3f4f6;
    color: #4b5563;
    border: none;
  }

  .error {
    color: #ef4444;
    background: #fef2f2;
    padding: 0.75rem;
    border-radius: 8px;
    margin-bottom: 1.5rem;
    border: 1px solid #fee2e2;
  }
</style>
