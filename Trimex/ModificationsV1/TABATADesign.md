<!DOCTYPE html>

<html class="dark" lang="en"><head>
<meta charset="utf-8"/>
<meta content="width=device-width, initial-scale=1.0" name="viewport"/>
<title>TRIMEX - Tabata Configuration</title>
<script src="https://cdn.tailwindcss.com?plugins=forms,container-queries"></script>
<link href="https://fonts.googleapis.com/css2?family=Space+Grotesk:wght@300;400;500;600;700;900&amp;family=Inter:wght@300;400;500;600;700&amp;display=swap" rel="stylesheet"/>
<link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:wght,FILL@100..700,0..1&amp;display=swap" rel="stylesheet"/>
<link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:wght,FILL@100..700,0..1&amp;display=swap" rel="stylesheet"/>
<script id="tailwind-config">
      tailwind.config = {
        darkMode: "class",
        theme: {
          extend: {
            colors: {
              "error": "#ff7351",
              "inverse-primary": "#516700",
              "primary-container": "#cafd00",
              "outline-variant": "#484847",
              "primary-fixed": "#cafd00",
              "on-surface-variant": "#adaaaa",
              "secondary-container": "#006875",
              "surface-variant": "#262626",
              "primary": "#f3ffca",
              "on-secondary": "#004d57",
              "error-container": "#b92902",
              "secondary-dim": "#00d4ec",
              "on-error": "#450900",
              "background": "#0e0e0e",
              "primary-dim": "#beee00",
              "inverse-surface": "#fcf9f8",
              "surface-dim": "#0e0e0e",
              "on-primary-fixed-variant": "#526900",
              "on-primary": "#516700",
              "error-dim": "#d53d18",
              "tertiary-fixed": "#fce047",
              "tertiary": "#ffeea5",
              "on-secondary-container": "#e8fbff",
              "surface-container-lowest": "#000000",
              "secondary-fixed-dim": "#00d7f0",
              "surface-bright": "#2c2c2c",
              "surface-container-highest": "#262626",
              "tertiary-fixed-dim": "#edd13a",
              "secondary": "#00e3fd",
              "on-tertiary-container": "#5d5000",
              "on-background": "#ffffff",
              "primary-fixed-dim": "#beee00",
              "on-error-container": "#ffd2c8",
              "on-primary-container": "#4a5e00",
              "surface-tint": "#f3ffca",
              "surface-container-high": "#201f1f",
              "on-primary-fixed": "#3a4a00",
              "on-tertiary": "#665800",
              "on-secondary-fixed-variant": "#005964",
              "surface-container-low": "#131313",
              "on-tertiary-fixed": "#483d00",
              "on-secondary-fixed": "#003a42",
              "tertiary-dim": "#edd13a",
              "on-tertiary-fixed-variant": "#685900",
              "outline": "#767575",
              "on-surface": "#ffffff",
              "surface": "#0e0e0e",
              "tertiary-container": "#fce047",
              "inverse-on-surface": "#565555",
              "secondary-fixed": "#26e6ff",
              "surface-container": "#1a1919"
            },
            fontFamily: {
              "headline": ["Space Grotesk"],
              "body": ["Inter"],
              "label": ["Inter"]
            },
            borderRadius: {"DEFAULT": "0.25rem", "lg": "0.5rem", "xl": "0.75rem", "full": "9999px"},
          },
        },
      }
    </script>
<style>
        .material-symbols-outlined {
            font-variation-settings: 'FILL' 0, 'wght' 400, 'GRAD' 0, 'opsz' 24;
        }
        .kinetic-pulse {
            background: linear-gradient(135deg, #f3ffca 0%, #cafd00 100%);
        }
        .tonal-layer-top {
            background: linear-gradient(to bottom, rgba(14,14,14,1) 0%, rgba(14,14,14,0.7) 100%);
        }
    </style>
<style>
    body {
      min-height: max(884px, 100dvh);
    }
  </style>
  </head>
<body class="bg-background text-on-background font-body selection:bg-primary-container selection:text-on-primary-container">
<!-- Top Navigation Bar -->
<header class="bg-[#0e0e0e]/70 backdrop-blur-md fixed top-0 w-full z-50 flex items-center justify-between px-6 h-16 w-full">
<div class="flex items-center gap-4">
<button class="text-[#CCFF00] hover:text-[#CCFF00] transition-colors kinetic-pulse-on-click">
<span class="material-symbols-outlined" data-icon="arrow_back">arrow_back</span>
</button>
<span class="font-['Space_Grotesk'] font-bold uppercase tracking-tighter text-2xl font-black text-[#CCFF00] tracking-widest">TRIMEX</span>
</div>
<div class="hidden md:flex gap-8 items-center">
<a class="font-['Inter'] font-medium text-[10px] uppercase text-[#CCFF00] scale-95 transition-transform" href="#">Timer</a>
<a class="font-['Inter'] font-medium text-[10px] uppercase text-[#adaaaa] hover:text-[#CCFF00] transition-colors" href="#">Workouts</a>
<a class="font-['Inter'] font-medium text-[10px] uppercase text-[#adaaaa] hover:text-[#CCFF00] transition-colors" href="#">Stats</a>
<a class="font-['Inter'] font-medium text-[10px] uppercase text-[#adaaaa] hover:text-[#CCFF00] transition-colors" href="#">Profile</a>
</div>
<button class="text-[#adaaaa] md:hidden">
<span class="material-symbols-outlined" data-icon="menu">menu</span>
</button>
</header>
<main class="pt-24 pb-32 px-6 max-w-2xl mx-auto min-h-screen flex flex-col items-center">
<!-- Hero Header -->
<section class="text-center mb-12 w-full">
<h1 class="font-headline font-black text-6xl md:text-8xl tracking-tighter mb-2 text-on-surface">TABATA</h1>
<p class="font-body text-on-surface-variant uppercase tracking-widest text-xs">Configure your tabata as you like</p>
</section>
<!-- Bento Configuration Grid -->
<div class="grid grid-cols-1 md:grid-cols-2 gap-4 w-full">
<!-- Rounds Selector -->
<div class="bg-surface-container-low rounded-xl p-6 flex flex-col justify-between group cursor-pointer hover:bg-surface-container-high transition-all active:scale-95">
<span class="font-headline font-bold text-on-surface-variant tracking-widest text-sm mb-8">ROUNDS</span>
<div class="flex items-baseline gap-2">
<span class="font-headline font-black text-7xl text-primary-container">8</span>
<span class="font-headline font-bold text-on-surface-variant text-xl">SETS</span>
</div>
<div class="mt-4 flex justify-end">
<span class="material-symbols-outlined text-primary-container opacity-0 group-hover:opacity-100 transition-opacity" data-icon="edit">edit</span>
</div>
</div>
<!-- Work Time -->
<div class="bg-surface-container-low rounded-xl p-6 flex flex-col justify-between group cursor-pointer hover:bg-surface-container-high transition-all active:scale-95 border-l-4 border-primary-container">
<div class="flex items-center gap-2 mb-8">
<span class="material-symbols-outlined text-primary-container" data-icon="timer">timer</span>
<span class="font-headline font-bold text-on-surface-variant tracking-widest text-sm">WORK TIME</span>
</div>
<span class="font-headline font-black text-6xl text-on-surface tracking-tight">00:20</span>
<div class="mt-4 flex justify-end">
<span class="material-symbols-outlined text-primary-container opacity-0 group-hover:opacity-100 transition-opacity" data-icon="tune">tune</span>
</div>
</div>
<!-- Rest Time -->
<div class="bg-surface-container-low rounded-xl p-6 flex flex-col justify-between group cursor-pointer hover:bg-surface-container-high transition-all active:scale-95 border-l-4 border-secondary">
<div class="flex items-center gap-2 mb-8">
<span class="material-symbols-outlined text-secondary" data-icon="rest_pace">pace</span>
<span class="font-headline font-bold text-on-surface-variant tracking-widest text-sm">REST TIME</span>
</div>
<span class="font-headline font-black text-6xl text-on-surface tracking-tight">00:10</span>
<div class="mt-4 flex justify-end">
<span class="material-symbols-outlined text-secondary opacity-0 group-hover:opacity-100 transition-opacity" data-icon="tune">tune</span>
</div>
</div>
</div>
<!-- Total Stats Bar -->
<div class="w-full mt-8 bg-surface-container-highest rounded-xl p-4 flex justify-around items-center">
<div class="text-center">
<p class="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest">Total Duration</p>
<p class="font-headline font-black text-lg text-primary-container">04:00</p>
</div>
<div class="h-8 w-[1px] bg-outline-variant/30"></div>
<div class="text-center">
<p class="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest">Intensity</p>
<p class="font-headline font-black text-lg text-secondary">MAX</p>
</div>
</div>
</main>
<!-- Fixed Action Footer -->
<div class="fixed bottom-0 left-0 w-full z-40 bg-background/80 backdrop-blur-xl px-6 pb-10 pt-4 md:static md:bg-transparent md:px-0 md:pb-24">
<div class="max-w-2xl mx-auto">
<button class="w-full kinetic-pulse text-[#0e0e0e] font-headline font-black text-xl py-6 rounded-xl flex items-center justify-center gap-3 shadow-[0_0_40px_rgba(204,255,0,0.3)] hover:scale-[1.02] active:scale-[0.98] transition-all group">
                START WORKOUT
                <span class="material-symbols-outlined font-black transition-transform group-hover:rotate-12" data-icon="bolt" data-weight="fill" style="font-variation-settings: 'FILL' 1;">bolt</span>
</button>
</div>
</div>
<!-- Bottom Nav Bar (Mobile Only) -->
<nav class="md:hidden bg-[#0e0e0e]/80 backdrop-blur-xl fixed bottom-0 w-full rounded-t-[1rem] z-50 shadow-[0_-8px_32px_rgba(204,255,0,0.1)] flex justify-around items-center px-4 pb-6 pt-3 w-full">
<a class="flex items-center justify-center bg-[#CCFF00] text-[#0e0e0e] rounded-xl p-3 shadow-[0_0_20px_rgba(204,255,0,0.4)]" href="#">
<span class="material-symbols-outlined" data-icon="timer">timer</span>
</a>
<a class="flex items-center justify-center text-[#adaaaa] p-3 hover:bg-[#202020] rounded-xl transition-all" href="#">
<span class="material-symbols-outlined" data-icon="fitness_center">fitness_center</span>
</a>
<a class="flex items-center justify-center text-[#adaaaa] p-3 hover:bg-[#202020] rounded-xl transition-all" href="#">
<span class="material-symbols-outlined" data-icon="leaderboard">leaderboard</span>
</a>
<a class="flex items-center justify-center text-[#adaaaa] p-3 hover:bg-[#202020] rounded-xl transition-all" href="#">
<span class="material-symbols-outlined" data-icon="person">person</span>
</a>
</nav>
<!-- Decorative Gradients -->
<div class="fixed top-[-10%] left-[-10%] w-[40%] h-[40%] bg-primary-container/10 blur-[120px] rounded-full -z-10"></div>
<div class="fixed bottom-[-10%] right-[-10%] w-[40%] h-[40%] bg-secondary/10 blur-[120px] rounded-full -z-10"></div>
</body></html>