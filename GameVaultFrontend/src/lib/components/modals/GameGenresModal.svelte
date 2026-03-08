<script>
  import BaseModal from './BaseModal.svelte';
  import { tableStore } from '$lib/stores/tableStore.svelte';
  import { gamesApi } from '$lib/api/games';
  import { genresApi } from '$lib/api/genres';
  import { onMount } from 'svelte';
  import { mapError } from '$lib/utils/errorMapper';

  const game = tableStore.currentItem;
  let gameGenres = $state([]);
  let allGenres = $state([]);
  let selectedGenre = $state('');
  let loading = $state(true);
  let actionLoading = $state(false);
  let error = $state(null);

  onMount(async () => {
    await Promise.all([fetchGameGenres(), fetchAllGenres()]);
    loading = false;
  });

  async function fetchGameGenres() {
    try {
      gameGenres = await gamesApi.getGenres(game.title);
    } catch (err) {
      error = mapError(err);
    }
  }

  async function fetchAllGenres() {
    try {
      allGenres = await genresApi.getAll();
    } catch (err) {
      error = mapError(err);
    }
  }

  async function addGenre() {
    if (!selectedGenre || actionLoading) return;
    actionLoading = true;
    error = null;
    try {
      await gamesApi.addGenre(game.title, selectedGenre);
      selectedGenre = '';
      await fetchGameGenres();
    } catch (err) {
      error = mapError(err);
    } finally {
      actionLoading = false;
    }
  }

  async function removeGenre(genreName) {
    if (actionLoading) return;
    actionLoading = true;
    error = null;
    try {
      await gamesApi.removeGenre(game.title, genreName);
      await fetchGameGenres();
    } catch (err) {
      error = mapError(err);
    } finally {
      actionLoading = false;
    }
  }

  const availableGenres = $derived(
    allGenres.filter(g => !gameGenres.some(gg => gg.name === g.name))
  );
</script>

<BaseModal 
  title={`Жанры: ${game?.title}`} 
  onclose={() => tableStore.closeModal()}
>
  <div class="genre-manager">
    {#if loading}
      <div class="loader">
        <div class="spinner small"></div>
        <span>Загрузка...</span>
      </div>
    {:else}
      <div class="current-genres">
        <h3>Текущие жанры</h3>
        {#if gameGenres.length === 0}
          <p class="empty">У этой игры пока нет жанров</p>
        {:else}
          <div class="genre-tags">
            {#each gameGenres as genre}
              <div class="genre-tag">
                <span>{genre.name}</span>
                <button 
                  class="remove-btn" 
                  onclick={() => removeGenre(genre.name)}
                  disabled={actionLoading}
                  title="Удалить жанр"
                >
                  &times;
                </button>
              </div>
            {/each}
          </div>
        {/if}
      </div>

      <div class="add-genre">
        <h3>Добавить жанр</h3>
        <div class="add-controls">
          <select bind:value={selectedGenre} disabled={actionLoading || availableGenres.length === 0}>
            <option value="">Выберите жанр...</option>
            {#each availableGenres as genre}
              <option value={genre.name}>{genre.name}</option>
            {/each}
          </select>
          <button 
            class="btn-primary" 
            onclick={addGenre} 
            disabled={!selectedGenre || actionLoading}
          >
            {actionLoading ? '...' : 'Добавить'}
          </button>
        </div>
        {#if availableGenres.length === 0 && allGenres.length > 0 && gameGenres.length === allGenres.length}
          <p class="hint">Добавлены все доступные жанры</p>
        {/if}
      </div>
    {/if}

    {#if error}
      <div class="error">{error}</div>
    {/if}
  </div>
</BaseModal>

<style>
  .genre-manager {
    min-width: 400px;
    display: flex;
    flex-direction: column;
    gap: 2rem;
  }

  h3 {
    margin: 0 0 1rem;
    font-size: 0.875rem;
    text-transform: uppercase;
    letter-spacing: 0.05em;
    color: #64748b;
  }

  .loader {
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 1rem;
    padding: 2rem;
    color: #64748b;
  }

  .genre-tags {
    display: flex;
    flex-wrap: wrap;
    gap: 0.75rem;
  }

  .genre-tag {
    display: flex;
    align-items: center;
    gap: 0.5rem;
    background: #f1f5f9;
    color: #1e293b;
    padding: 0.5rem 0.875rem;
    border-radius: 100px;
    font-size: 0.9rem;
    font-weight: 500;
    border: 1px solid #e2e8f0;
  }

  .remove-btn {
    background: none;
    border: none;
    color: #94a3b8;
    cursor: pointer;
    font-size: 1.25rem;
    line-height: 1;
    padding: 0;
    display: flex;
    align-items: center;
    justify-content: center;
    transition: color 0.2s;
  }

  .remove-btn:hover:not(:disabled) {
    color: #ef4444;
  }

  .empty {
    color: #94a3b8;
    font-style: italic;
    font-size: 0.9rem;
  }

  .add-controls {
    display: flex;
    gap: 0.75rem;
  }

  select {
    flex: 1;
    padding: 0.75rem 1rem;
    border: 1px solid #e2e8f0;
    border-radius: 12px;
    background: #f8fafc;
    font-size: 0.95rem;
  }

  .btn-primary {
    padding: 0.75rem 1.5rem;
    background: #6366f1;
    color: white;
    border: none;
    border-radius: 12px;
    font-weight: 700;
    cursor: pointer;
    white-space: nowrap;
  }

  .btn-primary:disabled {
    opacity: 0.6;
    cursor: not-allowed;
  }

  .error {
    color: #ef4444;
    font-size: 0.875rem;
    background: #fef2f2;
    padding: 1rem;
    border-radius: 12px;
    border: 1px solid #fee2e2;
  }

  .hint {
    margin-top: 0.5rem;
    font-size: 0.8rem;
    color: #64748b;
  }

  .spinner.small {
    width: 20px;
    height: 20px;
    border: 2px solid #f3f3f3;
    border-top: 2px solid #6366f1;
    border-radius: 50%;
    animation: spin 1s linear infinite;
  }

  @keyframes spin {
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
  }
</style>
