<template>
  <header class="relative border-b border-white/5 bg-slate-900/80 backdrop-blur-lg">
    <!-- Ambient glow -->
    <div class="absolute inset-0 bg-gradient-to-r from-sky-600/5 via-transparent to-cyan-600/5 pointer-events-none" />

    <div class="relative mx-auto max-w-4xl px-6 py-4 flex items-center justify-between">
      <div class="flex items-center gap-3">
        <!-- Logo mark -->
        <div class="flex items-center justify-center w-8 h-8 rounded-lg bg-sky-600/20 border border-sky-500/30">
          <svg class="w-4 h-4 text-sky-400" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
            <path stroke-linecap="round" stroke-linejoin="round" d="m21 21-5.197-5.197m0 0A7.5 7.5 0 1 0 5.196 5.196a7.5 7.5 0 0 0 10.607 10.607Z" />
          </svg>
        </div>
        <div>
          <h1 class="text-sm font-semibold text-slate-100 tracking-wide">Enterprise Search</h1>
          <p class="text-xs text-slate-500">Powered by ASE</p>
        </div>
      </div>

      <div class="flex items-center gap-2">
        <span class="inline-flex items-center gap-1.5 text-xs text-slate-500">
          <span class="w-1.5 h-1.5 rounded-full bg-emerald-500 animate-pulse-slow" />
          Connected
        </span>
      </div>
    </div>
  </header>
</template>
