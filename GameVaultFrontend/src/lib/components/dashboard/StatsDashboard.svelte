<script>
  /**
   * @component StatsDashboard
   * Компонент главной страницы, отображающий статистику по всей базе данных.
   */
  import { onMount } from 'svelte';
  import { statisticsApi } from '$lib/api/statistics';
  import { fade, fly } from 'svelte/transition';

  let stats = $state(null);
  let loading = $state(true);
  let error = $state(null);

  onMount(async () => {
    try {
      stats = await statisticsApi.getStats();
    } catch (err) {
      error = 'Не удалось загрузить статистику';
      console.error(err);
    } finally {
      loading = false;
    }
  });

  const cards = $derived([
    { label: 'Всего игр', value: stats?.totalGames, icon: '🎮', color: '#6366f1' },
    { label: 'Игроков', value: stats?.totalPlayers, icon: '👥', color: '#22c55e' },
    { label: 'Жанров', value: stats?.totalGenres, icon: '🏷️', color: '#eab308' },
    { label: 'Издателей', value: stats?.totalPublishers, icon: '🏢', color: '#a855f7' },
    { label: 'Стран', value: stats?.totalCountries, icon: '🌍', color: '#f43f5e' }
  ]);
</script>

<div class="stats-dashboard">
  {#if loading}
    <div class="loading-state" out:fade>
      <div class="spinner"></div>
      <p>Загрузка статистики...</p>
    </div>
  {:else if error}
    <div class="error-state" in:fade>
      <span class="error-icon">⚠️</span>
      <p>{error}</p>
    </div>
  {:else}
    <div class="stats-grid" in:fade>
      {#each cards as card, i}
        <div 
          class="stat-card" 
          in:fly={{ y: 20, delay: i * 100, duration: 800 }}
          style="--card-color: {card.color}"
        >
          <div class="card-left">
            <span class="card-label">{card.label}</span>
            <span class="card-value">{card.value ?? 0}</span>
          </div>
          <div class="card-right">
            <div class="icon-wrapper">
              <span class="icon">{card.icon}</span>
            </div>
          </div>
        </div>
      {/each}
    </div>
  {/if}
</div>

<style>
  .stats-dashboard {
    margin-bottom: 4rem;
    min-height: 140px;
  }

  .stats-grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
    gap: 1.5rem;
  }

  .stat-card {
    background: white;
    padding: 1.5rem;
    border-radius: 24px;
    display: flex;
    justify-content: space-between;
    align-items: center;
    border: 1px solid #f1f5f9;
    box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.05);
    transition: all 0.3s ease;
    position: relative;
    overflow: hidden;
  }

  .stat-card:hover {
    transform: translateY(-4px);
    box-shadow: 0 12px 20px -5px rgba(0, 0, 0, 0.1);
    border-color: var(--card-color);
  }

  .card-left {
    display: flex;
    flex-direction: column;
    gap: 0.25rem;
    z-index: 1;
  }

  .card-label {
    font-size: 0.875rem;
    font-weight: 600;
    color: #64748b;
    text-transform: uppercase;
    letter-spacing: 0.5px;
  }

  .card-value {
    font-size: 2rem;
    font-weight: 800;
    color: #1e293b;
  }

  .icon-wrapper {
    width: 48px;
    height: 48px;
    background: #f8fafc;
    border-radius: 14px;
    display: flex;
    align-items: center;
    justify-content: center;
    transition: all 0.3s ease;
  }

  .icon {
    font-size: 1.5rem;
  }

  .stat-card:hover .icon-wrapper {
    background: var(--card-color);
    transform: rotate(10deg);
  }

  .stat-card:hover .icon {
    filter: brightness(0) invert(1);
  }

  /* Loading State */
  .loading-state, .error-state {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    height: 140px;
    background: rgba(255, 255, 255, 0.5);
    border-radius: 24px;
    border: 2px dashed #e2e8f0;
    color: #64748b;
  }

  .spinner {
    width: 32px;
    height: 32px;
    border: 3px solid #f3f4f6;
    border-top: 3px solid #6366f1;
    border-radius: 50%;
    animation: spin 1s linear infinite;
    margin-bottom: 1rem;
  }

  @keyframes spin {
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
  }

  .error-icon {
    font-size: 2rem;
    margin-bottom: 0.5rem;
  }

  .error-state p {
    color: #ef4444;
    font-weight: 600;
  }
</style>
