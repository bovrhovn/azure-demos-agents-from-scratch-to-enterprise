<template>
  <div class="space-y-2">
    <form @submit.prevent="handleSubmit" class="flex gap-3" data-testid="search-form">
      <div class="flex-1 relative">
        <div class="absolute inset-y-0 left-0 pl-4 flex items-center pointer-events-none">
          <svg class="w-4 h-4 text-slate-500" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
            <path stroke-linecap="round" stroke-linejoin="round" d="m21 21-5.197-5.197m0 0A7.5 7.5 0 1 0 5.196 5.196a7.5 7.5 0 0 0 10.607 10.607Z" />
          </svg>
        </div>
        <input
          v-model="localQuery"
          type="text"
          placeholder="Search enterprise data..."
          class="input-field pl-11"
          :class="{ 'border-red-500 focus:border-red-500 focus:ring-red-500': hasError }"
          :disabled="isLoading"
          data-testid="search-input"
          @input="onInput"
          @keydown.enter.prevent="handleSubmit"
          autocomplete="off"
          spellcheck="false"
        />
      </div>

      <button
        type="submit"
        class="btn-primary flex items-center gap-2 whitespace-nowrap"
        :disabled="isLoading"
        data-testid="search-button"
      >
        <svg v-if="!isLoading" class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
          <path stroke-linecap="round" stroke-linejoin="round" d="m21 21-5.197-5.197m0 0A7.5 7.5 0 1 0 5.196 5.196a7.5 7.5 0 0 0 10.607 10.607Z" />
        </svg>
        <svg v-else class="w-4 h-4 animate-spin" fill="none" viewBox="0 0 24 24">
          <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" />
          <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z" />
        </svg>
        {{ isLoading ? 'Searching\u2026' : 'Search' }}
      </button>
    </form>

    <!-- Validation error -->
    <Transition name="error">
      <p
        v-if="validationError"
        class="text-sm text-red-400 flex items-center gap-1.5"
        data-testid="validation-error"
        role="alert"
      >
        <svg class="w-3.5 h-3.5 shrink-0" fill="currentColor" viewBox="0 0 20 20">
          <path fill-rule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-8-5a.75.75 0 01.75.75v4.5a.75.75 0 01-1.5 0v-4.5A.75.75 0 0110 5zm0 10a1 1 0 100-2 1 1 0 000 2z" clip-rule="evenodd" />
        </svg>
        {{ validationError }}
      </p>
    </Transition>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'

const props = defineProps<{
  modelValue: string
  isLoading: boolean
  validationError: string | null
}>()

const emit = defineEmits<{
  'update:modelValue': [value: string]
  'submit': [query: string]
  'clearValidation': []
}>()

const localQuery = ref(props.modelValue)
const hasError = ref(false)

watch(() => props.validationError, (err) => {
  hasError.value = !!err
})

watch(localQuery, (value) => {
  emit('update:modelValue', value)
})

function onInput(): void {
  if (props.validationError) {
    emit('clearValidation')
    hasError.value = false
  }
}

function handleSubmit(): void {
  emit('submit', localQuery.value)
}
</script>

<style scoped>
.error-enter-active,
.error-leave-active {
  transition: all 0.2s ease;
}
.error-enter-from,
.error-leave-to {
  opacity: 0;
  transform: translateY(-4px);
}
</style>
