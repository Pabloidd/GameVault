<script>
  import logo from '$lib/images/Pablo\'s_product.png';
</script>

<footer class="footer">
  <div class="container">
    <div class="footer-content">
      <div class="footer-logo">
        <img src={logo} alt="Pablo's Product Logo" class="logo-img" />
      </div>
      <p class="copyright">&copy; 2026 <span class="brand">GameVault</span> Admin Panel. All rights reserved.</p>
      <div class="footer-links">
        <span class="status">System Online</span>
      </div>
    </div>
  </div>
</footer>

<style>
  .footer {
    padding: 3rem 2rem;
    background: #ffffff;
    color: #6b7280;
    border-top: 1px solid #f3f4f6;
  }
  
  .container {
    max-width: 1400px;
    margin: 0 auto;
  }

  .footer-content {
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 1.5rem;
  }

  .footer-logo {
    margin-bottom: 0.5rem;
  }

  .logo-img {
    height: 60px;
    width: auto;
    object-fit: contain;
    filter: grayscale(0.2) opacity(0.8);
    transition: all 0.3s ease;
  }

  .logo-img:hover {
    filter: grayscale(0) opacity(1);
    transform: scale(1.05);
  }

  .copyright {
    font-size: 0.9rem;
  }

  .brand {
    font-weight: 600;
    color: #1a1a1a;
  }

  .status {
    font-size: 0.8rem;
    display: flex;
    align-items: center;
    gap: 0.5rem;
  }

  .status::before {
    content: '';
    display: inline-block;
    width: 6px;
    height: 6px;
    background: #10b981;
    border-radius: 50%;
    box-shadow: 0 0 8px rgba(16, 185, 129, 0.6);
  }
</style>