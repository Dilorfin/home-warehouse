<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <header>
    <nav class="navbar bg-body-tertiary">
      <div class="container-fluid">
        <div class="navbar-nav">
          <RouterLink to="/" class="nav-link active">Scanner</RouterLink>
          <RouterLink to="/qr-generator"  class="nav-link">QR Generator</RouterLink>
        </div>
      </div>
    </nav>
  </header>

  <main class="container">
    <RouterView />
  </main>
</template>
