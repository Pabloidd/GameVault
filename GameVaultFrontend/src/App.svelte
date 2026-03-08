<!-- src/App.svelte -->
<script>
  import { Router, Route } from 'svelte-routing';
  import Header from './lib/components/layout/Header.svelte';
  import Footer from './lib/components/layout/Footer.svelte';
  import HomePage from './routes/+page.svelte';
  import TablePage from './routes/[table]/+page.svelte';

  export let url = "";
</script>

<Router {url}>
  <Header />
  
  <main>
    <Route path="/">
      <HomePage />
    </Route>
    <Route path="/:table">
      {#snippet children({ params })}
        <TablePage table={params.table} />
      {/snippet}
    </Route>

  </main>

  <Footer />
</Router>



<style>
  main {
    min-height: calc(100vh - 120px);
    background: #f8fafc;
  }
</style>

