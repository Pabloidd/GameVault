<script>
  import BaseModal from './BaseModal.svelte';
  import { tableStore } from '$lib/stores/tableStore.svelte';
  import { playersApi } from '$lib/api/players';
  import { onMount } from 'svelte';
  import { mapError } from '$lib/utils/errorMapper';

  const player = tableStore.currentItem;
  let games = $state([]);
  let loading = $state(true);
  let error = $state(null);

  onMount(async () => {
    try {
      games = await playersApi.getGames(player.nickname);
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

<BaseModal 
  title={`Игры игрока: ${player?.nickname}`} 
  onclose={() => tableStore.closeModal()}
>
  <div class="games-view">
    {#if loading}
      <div class="loader">
        <div class="spinner small"></div>
        <span>Загрузка игр...</span>
      </div>
    {:else if error}
      <div class="error">{error}</div>
    {:else}
      {#if games.length === 0}
        <div class="empty-state">
          <div class="icon">🎮</div>
          <p>У этого игрока пока нет игр</p>
        </div>
      {:else}
        <div class="games-list">
          {#each games as game}
            <div class="game-card">
              <div class="icon">🎲</div>
              <div class="info">
                <span class="title">{game.title}</span>
                <span class="details">{game.company} • {game.weight} ГБ</span>
                <span class="date">Релиз: {formatDate(game.release_date)}</span>
              </div>
            </div>
          {/each}
        </div>
      {/if}
    {/if}
  </div>
</BaseModal>

<style>
  .games-view {
    min-width: 450px;
    max-height: 60vh;
    overflow-y: auto;
  }

  .loader {
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 1rem;
    padding: 3rem;
    color: #64748b;
  }

  .games-list {
    display: flex;
    flex-direction: column;
    gap: 1rem;
  }

  .game-card {
    display: flex;
    align-items: center;
    gap: 1rem;
    padding: 1rem;
    background: #f8fafc;
    border: 1px solid #e2e8f0;
    border-radius: 16px;
    transition: all 0.2s;
  }

  .game-card:hover {
    transform: translateX(4px);
    border-color: #6366f1;
    background: white;
    box-shadow: 0 4px 12px rgba(99, 102, 241, 0.05);
  }

  .icon {
    width: 48px;
    height: 48px;
    background: linear-gradient(135deg, #6366f1, #8b5cf6);
    color: white;
    border-radius: 12px;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 1.5rem;
    flex-shrink: 0;
  }

  .info {
    display: flex;
    flex-direction: column;
    gap: 0.125rem;
  }

  .title {
    font-weight: 700;
    color: #1e293b;
    font-size: 1rem;
  }

  .details {
    font-size: 0.85rem;
    color: #64748b;
  }

  .date {
    font-size: 0.75rem;
    color: #94a3b8;
    margin-top: 0.25rem;
  }

  .empty-state {
    text-align: center;
    padding: 4rem 2rem;
    color: #94a3b8;
  }

  .empty-state .icon {
    font-size: 3rem;
    margin-bottom: 1rem;
    background: none;
    color: inherit;
  }

  .error {
    color: #ef4444;
    background: #fef2f2;
    padding: 1.5rem;
    border-radius: 16px;
    border: 1px solid #fee2e2;
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
