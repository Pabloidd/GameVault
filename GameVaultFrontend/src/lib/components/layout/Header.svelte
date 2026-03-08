<script>
  import { Link } from 'svelte-routing';
</script>

<header class="header">
  <div class="header-container">
    <Link to="/" class="logo">
      <div class="logo-icon-wrapper">
        <span class="logo-emoji">🎮</span>
      </div>
      <span class="logo-text">GameVault</span>
    </Link>
    
    <nav class="nav">
      <Link to="/" class="nav-link" getProps={({ isCurrent }) => ({ class: isCurrent ? 'nav-link active' : 'nav-link' })}>
        <span class="nav-icon">🏠</span>
        <span>Главная</span>
      </Link>
    </nav>
  </div>
</header>

<style>
  .header {
    background: rgba(255, 255, 255, 0.8);
    backdrop-filter: blur(12px);
    -webkit-backdrop-filter: blur(12px);
    border-bottom: 1px solid rgba(0, 0, 0, 0.05);
    color: #1a1a1a;
    position: sticky;
    top: 0;
    z-index: 1000;
    transition: all 0.3s ease;
  }
  
  .header-container {
    max-width: 1400px;
    margin: 0 auto;
    padding: 0.75rem 2rem;
    display: flex;
    align-items: center;
    justify-content: space-between;
  }
  
  :global(.logo) {
    display: flex;
    align-items: center;
    gap: 0.75rem;
    text-decoration: none;
    color: inherit;
    transition: transform 0.2s ease;
  }
  
  :global(.logo:hover) {
    transform: translateY(-1px);
  }

  .logo-icon-wrapper {
    background: linear-gradient(135deg, #6366f1 0%, #a855f7 100%);
    width: 44px;
    height: 44px;
    border-radius: 12px;
    display: flex;
    align-items: center;
    justify-content: center;
    box-shadow: 0 4px 12px rgba(99, 102, 241, 0.3);
  }

  .logo-emoji {
    font-size: 1.5rem;
    line-height: 1;
  }
  
  .logo-text {
    font-size: 1.5rem;
    font-weight: 800;
    letter-spacing: -0.5px;
    background: linear-gradient(135deg, #1a1a1a 0%, #4a4a4a 100%);
    background-clip: text;
    -webkit-background-clip: text;
    -webkit-text-fill-color: transparent;
  }
  
  .nav {
    display: flex;
    gap: 0.5rem;
  }
  
  :global(.nav-link) {
    display: flex;
    align-items: center;
    gap: 0.5rem;
    color: #4b5563;
    text-decoration: none;
    padding: 0.6rem 1.2rem;
    border-radius: 10px;
    transition: all 0.2s cubic-bezier(0.4, 0, 0.2, 1);
    font-weight: 500;
    font-size: 0.95rem;
  }
  
  :global(.nav-link:hover) {
    background: rgba(0, 0, 0, 0.03);
    color: #1a1a1a;
  }
  
  :global(.nav-link.active) {
    background: #f3f4f6;
    color: #6366f1;
    font-weight: 600;
  }
</style>