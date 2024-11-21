import { createRouter, createWebHistory } from 'vue-router'
import StorageView from '@/views/StorageView.vue'
import OpenStorageView from '@/views/OpenStorageView.vue'
import StoragesListView from '@/views/StoragesListView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'open-storage',
      component: OpenStorageView,
    },
    {
      path: '/storages',
      name: 'storages-list',
      component: StoragesListView,
    },
    {
      path: '/storage/:id',
      name: 'storage',
      component: StorageView,
    }
  ],
})

export default router
