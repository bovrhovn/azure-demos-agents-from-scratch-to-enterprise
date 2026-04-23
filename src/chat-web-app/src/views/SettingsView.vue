<template>
  <div class="flex flex-col min-h-screen">
    <AppHeader />

    <main class="flex-1 mx-auto w-full max-w-2xl px-6 py-12">
      <div class="mb-10 animate-fade-in">
        <h2 class="text-2xl font-semibold text-slate-100 mb-1">Settings</h2>
        <p class="text-slate-400 text-sm">Configure the connection to your ASE backend.</p>
      </div>

      <div class="glass-card p-6 animate-slide-up">
        <h3 class="text-sm font-medium text-slate-300 mb-4 flex items-center gap-2">
          <svg class="w-4 h-4 text-sky-400" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
            <path stroke-linecap="round" stroke-linejoin="round" d="M13.19 8.688a4.5 4.5 0 0 1 1.242 7.244l-4.5 4.5a4.5 4.5 0 0 1-6.364-6.364l1.757-1.757m13.35-.622 1.757-1.757a4.5 4.5 0 0 0-6.364-6.364l-4.5 4.5a4.5 4.5 0 0 0 1.242 7.244" />
          </svg>
          Backend URL
        </h3>

        <form @submit.prevent="handleSave" class="space-y-4">
          <div>
            <label for="baseUrl" class="block text-xs text-slate-400 mb-1.5">
              API Base URL
            </label>
            <input
              id="baseUrl"
              v-model="draft"
              type="url"
              placeholder="https://localhost:5066"
              class="input-field"
              :class="{ 'border-red-500 focus:border-red-500 focus:ring-red-500': urlError }"
              autocomplete="off"
              spellcheck="false"
              @input="urlError = ''"
            />
            <p v-if="urlError" class="mt-1.5 text-xs text-red-400">{{ urlError }}</p>
            <p class="mt-1.5 text-xs text-slate-600">
              Default: <code class="text-slate-500">{{ defaultUrl }}</code>
            </p>
          </div>

          <div class="flex items-center gap-3 pt-1">
            <button type="submit" class="btn-primary text-sm px-5 py-2.5">
              Save
            </button>
            <button
              type="button"
              class="text-sm text-slate-400 hover:text-slate-200 transition-colors"
              @click="handleReset"
            >
              Reset to default
            </button>
          </div>
        </form>

        <!-- Success banner -->
        <Transition name="banner">
          <div
            v-if="saved"
            class="mt-4 flex items-center gap-2 text-sm text-emerald-400 bg-emerald-500/10 border border-emerald-500/20 rounded-lg px-4 py-2.5"
            role="status"
          >
            <svg class="w-4 h-4 shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
              <path stroke-linecap="round" stroke-linejoin="round" d="m4.5 12.75 6 6 9-13.5" />
            </svg>
            Settings saved. All searches will now use <strong class="font-medium ml-1">{{ baseUrl }}</strong>
          </div>
        </Transition>
      </div>

      <!-- Info card -->
      <div class="mt-4 glass-card p-4 animate-fade-in" style="animation-delay: 0.15s;">
        <p class="text-xs text-slate-500 leading-relaxed">
          <span class="text-slate-400 font-medium">Persistence:</span>
          The URL is saved in your browser's local storage and survives page reloads.
          To reset it, click "Reset to default" or clear site data from your browser.
        </p>
        <p class="mt-2 text-xs text-slate-500 leading-relaxed">
          <span class="text-slate-400 font-medium">Docker:</span>
          When running as a container, set the <code class="text-sky-500/80">VITE_API_BASE_URL</code>
          environment variable to override the compiled default.
          The settings page value always takes precedence in the browser.
        </p>
      </div>
    </main>

    <footer class="border-t border-white/5 py-4 px-6">
      <p class="text-center text-xs text-slate-600">
        ASE Enterprise Search · <span class="text-slate-500">{{ baseUrl }}</span>
      </p>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import AppHeader from '@/components/AppHeader.vue'
import { useSettings } from '@/composables/useSettings'

const { baseUrl, defaultUrl, saveBaseUrl, resetToDefault } = useSettings()

const draft = ref(baseUrl.value)
const urlError = ref('')
const saved = ref(false)

function handleSave(): void {
  const trimmed = draft.value.trim()
  if (!trimmed) {
    urlError.value = 'URL cannot be empty.'
    return
  }
  try {
    new URL(trimmed)
  } catch {
    urlError.value = 'Please enter a valid URL (e.g. https://api.myapp.com).'
    return
  }

  saveBaseUrl(trimmed)
  saved.value = true
  setTimeout(() => { saved.value = false }, 3000)
}

function handleReset(): void {
  resetToDefault()
  draft.value = defaultUrl
  saved.value = true
  setTimeout(() => { saved.value = false }, 3000)
}
</script>

<style scoped>
.banner-enter-active,
.banner-leave-active {
  transition: all 0.25s ease;
}
.banner-enter-from,
.banner-leave-to {
  opacity: 0;
  transform: translateY(-6px);
}
</style>
