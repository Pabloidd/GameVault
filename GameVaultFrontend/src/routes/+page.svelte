<script>
  import { TABLES } from '$lib/utils/constants';
  import { Link } from 'svelte-routing';
  import StatsDashboard from '$lib/components/dashboard/StatsDashboard.svelte';

  const tableIcons = {
    countries: '🌍',
    genres: '🏷️',
    games: '🎮',
    players: '👥',
    publishers: '🏢'
  };
</script>

<div class="home-page">
  <section class="hero">
    <h1>Выбор таблицы для работы</h1>
    <p>Управляйте данными вашей игровой платформы с помощью интуитивного интерфейса</p>
  </section>

  <StatsDashboard />

  <div class="grid">
    {#each TABLES as table}
      <Link to={`/${table.id}`} class="table-card">
        <div class="card-icon">
          <span class="emoji-icon">{tableIcons[table.id] || '📋'}</span>
        </div>
        <div class="card-content">
          <h3>{table.name}</h3>
          <p>Просмотр и редактирование записей таблицы {table.name.toLowerCase()}</p>
        </div>
        <div class="card-footer">
          <span>Перейти</span>
          <span class="arrow">→</span>
        </div>
      </Link>
    {/each}
  </div>
</div>

<style>
  .home-page {
    max-width: 1200px;
    margin: 0 auto;
    padding: 4rem 2rem;
  }

  .hero {
    text-align: center;
    margin-bottom: 4rem;
  }

  .hero h1 {
    font-size: 3rem;
    font-weight: 800;
    letter-spacing: -1px;
    margin-bottom: 1rem;
    background: linear-gradient(135deg, #1a1a1a 0%, #4a4a4a 100%);
    background-clip: text;
    -webkit-background-clip: text;
    -webkit-text-fill-color: transparent;
  }

  .hero p {
    font-size: 1.25rem;
    color: #6b7280;
    max-width: 600px;
    margin: 0 auto;
  }

  .grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
    gap: 2rem;
  }

  :global(.table-card) {
    background: white;
    padding: 2rem;
    border-radius: 24px;
    text-decoration: none;
    color: inherit;
    border: 1px solid #f3f4f6;
    display: flex;
    flex-direction: column;
    gap: 1.5rem;
    transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
    position: relative;
    overflow: hidden;
  }

  :global(.table-card::after) {
    content: '';
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: radial-gradient(circle at top right, rgba(99, 102, 241, 0.05) 0%, transparent 70%);
    opacity: 0;
    transition: opacity 0.4s ease;
  }

  :global(.table-card:hover) {
    transform: translateY(-8px);
    box-shadow: 0 20px 40px rgba(0, 0, 0, 0.08);
    border-color: rgba(99, 102, 241, 0.2);
  }

  :global(.table-card:hover::after) {
    opacity: 1;
  }

  .card-icon {
    width: 64px;
    height: 64px;
    background: #f8fafc;
    border-radius: 18px;
    display: flex;
    align-items: center;
    justify-content: center;
    transition: all 0.4s ease;
  }

  .emoji-icon {
    font-size: 2rem;
    line-height: 1;
  }

  :global(.table-card:hover .card-icon) {
    background: #6366f1;
    transform: scale(1.1);
  }

  :global(.table-card:hover .emoji-icon) {
    filter: brightness(0) invert(1);
    /* Note: Filter might not work perfectly for all emojis, 
       but we can also just keep it as is or use a different hover style. 
       Let's just keep it simple. */
    filter: none;
  }

  .card-content h3 {
    font-size: 1.5rem;
    font-weight: 700;
    margin-bottom: 0.5rem;
  }

  .card-content p {
    color: #6b7280;
    line-height: 1.6;
    font-size: 0.95rem;
  }

  .card-footer {
    margin-top: auto;
    display: flex;
    align-items: center;
    gap: 0.5rem;
    font-weight: 600;
    color: #6366f1;
    opacity: 0.8;
    transition: gap 0.2s ease;
  }

  :global(.table-card:hover .card-footer) {
    gap: 0.75rem;
    opacity: 1;
  }
</style>