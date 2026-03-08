<script>
  import { tableStore } from '$lib/stores/tableStore.svelte';
  import DataTable from '$lib/components/tables/DataTable.svelte';
  import CountryFormModal from '$lib/components/modals/CountryFormModal.svelte';
  import GenreFormModal from '$lib/components/modals/GenreFormModal.svelte';
  import PublisherFormModal from '$lib/components/modals/PublisherFormModal.svelte';
  import GameFormModal from '$lib/components/modals/GameFormModal.svelte';
  import GameGenresModal from '$lib/components/modals/GameGenresModal.svelte';
  import GamePlayersModal from '$lib/components/modals/GamePlayersModal.svelte';
  import PlayerFormModal from '$lib/components/modals/PlayerFormModal.svelte';
  import PlayerGamesModal from '$lib/components/modals/PlayerGamesModal.svelte';
  import GamesByRelationModal from '$lib/components/modals/GamesByRelationModal.svelte';
  import DeleteConfirmModal from '$lib/components/modals/DeleteConfirmModal.svelte';

  let { table } = $props();

  $effect(() => {
    if (table) {
      tableStore.init(table);
    }
  });
</script>

<div class="table-page">
  <div class="container">
    <header class="page-header">
      <div class="header-left">
        <div class="breadcrumb">
          <a href="/">Главная</a> / <span>{tableStore.currentTable?.name || '...'}</span>
        </div>
        <h1>{tableStore.currentTable?.name || 'Загрузка...'}</h1>
      </div>
      
      <div class="header-actions">
        <button class="btn-add" onclick={() => tableStore.openModal('create')}>
          <span class="plus-icon">+</span> Добавить запись
        </button>
      </div>
    </header>

    <DataTable />
  </div>
</div>

{#if tableStore.activeModal === 'create' || tableStore.activeModal === 'update'}
  {#if tableStore.currentTableId === 'countries'}
    <CountryFormModal />
  {:else if tableStore.currentTableId === 'genres'}
    <GenreFormModal />
  {:else if tableStore.currentTableId === 'publishers'}
    <PublisherFormModal />
  {:else if tableStore.currentTableId === 'games'}
    <GameFormModal />
  {:else if tableStore.currentTableId === 'players'}
    <PlayerFormModal />
  {/if}
{:else if tableStore.activeModal === 'gameGenres'}
  <GameGenresModal />
{:else if tableStore.activeModal === 'gamePlayers'}
  <GamePlayersModal />
{:else if tableStore.activeModal === 'playerGames'}
  <PlayerGamesModal />
{:else if tableStore.activeModal === 'gamesByRelation'}
  <GamesByRelationModal />
{/if}

{#if tableStore.activeModal === 'delete'}
  <DeleteConfirmModal />
{/if}

<style>
  .table-page {
    padding: 4rem 2rem;
    max-width: 1400px;
    margin: 0 auto;
  }

  .breadcrumb {
    margin-bottom: 1rem;
    font-size: 0.9rem;
    color: #6b7280;
  }

  .breadcrumb a {
    color: #6366f1;
    text-decoration: none;
  }

  .breadcrumb a:hover {
    text-decoration: underline;
  }

  .page-header {
    margin-bottom: 2rem;
    display: flex;
    align-items: flex-end;
    justify-content: space-between;
  }

  .btn-add {
    background: #6366f1;
    color: white;
    border: none;
    padding: 0.8rem 1.5rem;
    border-radius: 12px;
    font-weight: 700;
    display: flex;
    align-items: center;
    gap: 0.5rem;
    transition: all 0.2s cubic-bezier(0.4, 0, 0.2, 1);
    box-shadow: 0 4px 12px rgba(99, 102, 241, 0.2);
  }

  .btn-add:hover {
    background: #4f46e5;
    transform: translateY(-2px);
    box-shadow: 0 6px 16px rgba(99, 102, 241, 0.3);
  }

  .plus-icon {
    font-size: 1.25rem;
    line-height: 1;
  }
</style>
