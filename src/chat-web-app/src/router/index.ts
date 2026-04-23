import { createRouter, createWebHistory } from 'vue-router'
import SearchView from '@/views/SearchView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'search',
      component: SearchView,
      meta: { title: 'Basic Search' },
    },
    {
      path: '/advanced',
      name: 'advanced-search',
      component: () => import('@/views/AdvancedSearchView.vue'),
      meta: { title: 'Advanced Search' },
    },
    {
      path: '/settings',
      name: 'settings',
      component: () => import('@/views/SettingsView.vue'),
      meta: { title: 'Settings' },
    },
    {
      path: '/:pathMatch(.*)*',
      redirect: '/',
    },
  ],
})

router.afterEach((to) => {
  const title = (to.meta.title as string) ?? 'Enterprise Search'
  document.title = `${title} · ASE`
})

export default router
