import { createRouter, createWebHistory } from 'vue-router'
import ScannerView from '../views/ScannerView.vue'
import StorageView from '@/views/StorageView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'scanner',
      component: ScannerView,
    },
    {
      path: '/storage/:id',
      name: 'storage',
      component: StorageView,
    },
    {
      path: '/qr-generator',
      name: 'qr-generator',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/QRGeneratorView.vue'),
    },
  ],
})

export default router
