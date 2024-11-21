<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'

function setLocale(locale:string)
{
  localStorage.setItem('lang', locale);
}
</script>

<template>
  <header>
    <nav class="navbar bg-dark" data-bs-theme="dark">
      <div class="container-fluid">
        <div class="navbar-brand">
          <RouterLink to="/" class="nav-link active">Open Storage</RouterLink>
        </div>
        <div class="d-flex">
          <div class="dropdown">
            <button class="btn btn-secondary dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
              {{ $t('language.' + $i18n.locale) }}
            </button>
            <ul class="dropdown-menu">
              <li v-for="locale in $i18n.availableLocales">
                <a class="dropdown-item" @click="$i18n.locale = locale; setLocale(locale)">
                  {{ $t('language.' + locale) }}
                </a>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </nav>
  </header>

  <main class="container mt-2">
    <RouterView />
  </main>
</template>
