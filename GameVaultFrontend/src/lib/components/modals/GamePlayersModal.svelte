<script>
  import BaseModal from './BaseModal.svelte';
  import { tableStore } from '$lib/stores/tableStore.svelte';
  import { gamesApi } from '$lib/api/games';
  import { onMount } from 'svelte';
  import { mapError } from '$lib/utils/errorMapper';

  const game = tableStore.currentItem;
  let players = $state([]);
  let loading = $state(true);
  let error = $state(null);

  onMount(async () => {
    try {
      players = await gamesApi.getPlayers(game.title);
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
  title={`Игроки: ${game?.title}`} 
  onclose={() => tableStore.closeModal()}
>
  <div class="players-view">
    {#if loading}
      <div class="loader">
        <div class="spinner small"></div>
        <span>Поиск игроков...</span>
      </div>
    {:else if error}
      <div class="error">{error}</div>
    {:else}
      {#if players.length === 0}
        <div class="empty-state">
          <div class="icon">👤</div>
          <p>У этой игры пока нет игроков</p>
        </div>
      {:else}
        <div class="players-list">
          {#each players as player}
            <div class="player-card">
              <div class="avatar">
                {player.nickname.charAt(0).toUpperCase()}
              </div>
              <div class="info">
                <span class="nickname">{player.nickname}</span>
                <span class="details">Уровень: {player.level} • {player.email}</span>
                <span class="date">Зарегистрирован {formatDate(player.registrationDate)}</span>
              </div>
            </div>
          {/each}
        </div>
      {/if}
    {/if}
  </div>
</BaseModal>

<style>
  .players-view {
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

  .players-list {
    display: flex;
    flex-direction: column;
    gap: 1rem;
  }

  .player-card {
    display: flex;
    align-items: center;
    gap: 1rem;
    padding: 1rem;
    background: #f8fafc;
    border: 1px solid #e2e8f0;
    border-radius: 16px;
    transition: all 0.2s;
  }

  .player-card:hover {
    transform: translateX(4px);
    border-color: #6366f1;
    background: white;
    box-shadow: 0 4px 12px rgba(99, 102, 241, 0.05);
  }

  .avatar {
    width: 48px;
    height: 48px;
    background: linear-gradient(135deg, #6366f1, #8b5cf6);
    color: white;
    border-radius: 12px;
    display: flex;
    align-items: center;
    justify-content: center;
    font-weight: 700;
    font-size: 1.25rem;
    flex-shrink: 0;
  }

  .info {
    display: flex;
    flex-direction: column;
    gap: 0.125rem;
  }

  .nickname {
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
