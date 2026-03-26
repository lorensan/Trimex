<!DOCTYPE html>

<html class="dark" lang="en"><head>
<meta charset="utf-8"/>
<meta content="width=device-width, initial-scale=1.0" name="viewport"/>
<title>FOR TIME - Workout Configuration</title>
<script src="https://cdn.tailwindcss.com?plugins=forms,container-queries"></script>
<link href="https://fonts.googleapis.com/css2?family=Space+Grotesk:wght@300;400;500;600;700;800;900&amp;family=Inter:wght@300;400;500;600;700&amp;display=swap" rel="stylesheet"/>
<link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:wght,FILL@100..700,0..1&amp;display=swap" rel="stylesheet"/>
<link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:wght,FILL@100..700,0..1&amp;display=swap" rel="stylesheet"/>
<script id="tailwind-config">
      tailwind.config = {
        darkMode: "class",
        theme: {
          extend: {
            colors: {
              "secondary-fixed-dim": "#00d7f0",
              "tertiary": "#ffeea5",
              "outline": "#767575",
              "on-secondary-fixed": "#003a42",
              "outline-variant": "#484847",
              "on-tertiary-fixed": "#483d00",
              "secondary-fixed": "#26e6ff",
              "surface-dim": "#0e0e0e",
              "error-container": "#b92902",
              "on-tertiary": "#665800",
              "on-tertiary-container": "#5d5000",
              "on-surface-variant": "#adaaaa",
              "on-primary": "#516700",
              "primary": "#f3ffca",
              "primary-container": "#cafd00",
              "on-surface": "#ffffff",
              "surface-container-lowest": "#000000",
              "inverse-on-surface": "#565555",
              "surface-container-high": "#201f1f",
              "surface-container-low": "#131313",
              "error-dim": "#d53d18",
              "on-tertiary-fixed-variant": "#685900",
              "surface-tint": "#f3ffca",
              "primary-dim": "#beee00",
              "surface-variant": "#262626",
              "on-secondary-fixed-variant": "#005964",
              "inverse-primary": "#516700",
              "secondary-dim": "#00d4ec",
              "on-secondary-container": "#e8fbff",
              "tertiary-fixed": "#fce047",
              "tertiary-fixed-dim": "#edd13a",
              "primary-fixed-dim": "#beee00",
              "secondary": "#00e3fd",
              "primary-fixed": "#cafd00",
              "on-error": "#450900",
              "background": "#0e0e0e",
              "surface-bright": "#2c2c2c",
              "error": "#ff7351",
              "on-primary-fixed-variant": "#526900",
              "on-error-container": "#ffd2c8",
              "on-secondary": "#004d57",
              "secondary-container": "#006875",
              "surface-container": "#1a1919",
              "on-background": "#ffffff",
              "surface": "#0e0e0e",
              "inverse-surface": "#fcf9f8",
              "tertiary-container": "#fce047",
              "tertiary-dim": "#edd13a",
              "on-primary-fixed": "#3a4a00",
              "surface-container-highest": "#262626",
              "on-primary-container": "#4a5e00"
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
        .kinetic-gradient {
            background: linear-gradient(135deg, #f3ffca 0%, #cafd00 100%);
        }
        .glass-header {
            background: rgba(14, 14, 14, 0.7);
            backdrop-filter: blur(20px);
        }
    </style>
<style>
    body {
      min-height: max(884px, 100dvh);
    }
  </style>
  </head>
<body class="bg-background text-on-background font-body select-none">
<!-- TopAppBar -->
<header class="fixed top-0 w-full z-50 glass-header flex items-center justify-between px-6 h-16 w-full no-border bg-[#131313]">
<div class="flex items-center gap-4">
<button class="active:scale-95 duration-200 hover:opacity-80 transition-opacity">
<span class="material-symbols-outlined text-[#CCFF00]" data-icon="arrow_back">arrow_back</span>
</button>
<h1 class="font-['Space_Grotesk'] font-bold tracking-tighter uppercase text-xl text-[#CCFF00]">FOR TIME</h1>
</div>
<div class="text-[#CCFF00]">
<span class="material-symbols-outlined" data-icon="settings">settings</span>
</div>
</header>
<!-- Main Content Canvas -->
<main class="min-h-screen flex flex-col items-center justify-center px-6 pt-16 pb-32">
<!-- Hero Section -->
<div class="text-center mb-12">
<h2 class="font-headline text-5xl md:text-7xl font-extrabold tracking-tighter uppercase leading-none mb-2">
                FOR TIME
            </h2>
<p class="text-on-surface-variant font-medium tracking-widest uppercase text-xs">
                As fast as you can
            </p>
</div>
<!-- Main Selector Section -->
<div class="w-full max-w-xs flex flex-col items-center">
<label class="text-on-surface-variant font-label text-[10px] uppercase tracking-[0.2em] mb-4">TIME LIMIT</label>
<!-- Compact Card Selector -->
<div class="relative w-full aspect-square max-w-[240px] flex items-center justify-center rounded-3xl bg-surface-container-low overflow-hidden group">
<!-- Tonal layering instead of borders -->
<div class="absolute inset-0 bg-surface-container-high opacity-20 group-active:opacity-40 transition-opacity"></div>
<!-- Inner Depth Effect -->
<div class="absolute inset-4 rounded-2xl bg-surface-container-highest flex flex-col items-center justify-center shadow-inner">
<span class="font-headline text-6xl font-bold tracking-tight">15:00</span>
<span class="text-primary-container font-label text-[10px] mt-2 tracking-widest uppercase">MINUTES</span>
</div>
<!-- Subtle Decorative Accents (Asymmetry) -->
<div class="absolute top-4 right-4 h-2 w-2 rounded-full bg-secondary-fixed-dim blur-[1px]"></div>
</div>
<!-- Help Text -->
<p class="mt-6 text-on-surface-variant text-[11px] text-center leading-relaxed max-w-[200px]">
                Select 0 minutes if you want a manual stop with no cap
            </p>
</div>
<!-- Quick Select Pills (Editorial Pattern) -->
<div class="flex flex-wrap justify-center gap-2 mt-10">
<button class="px-4 py-2 rounded-full bg-surface-container-highest text-on-surface text-xs font-bold transition-all active:scale-90 active:bg-primary-container active:text-on-primary-container">5:00</button>
<button class="px-4 py-2 rounded-full bg-surface-container-highest text-on-surface text-xs font-bold transition-all active:scale-90 active:bg-primary-container active:text-on-primary-container">10:00</button>
<button class="px-4 py-2 rounded-full bg-primary-container text-on-primary-container text-xs font-bold shadow-[0_0_15px_rgba(204,255,0,0.3)]">15:00</button>
<button class="px-4 py-2 rounded-full bg-surface-container-highest text-on-surface text-xs font-bold transition-all active:scale-90 active:bg-primary-container active:text-on-primary-container">20:00</button>
</div>
</main>
<!-- Bottom Action Button Container -->
<div class="fixed bottom-24 left-0 w-full px-6 z-40">
<button class="kinetic-gradient w-full h-16 rounded-xl flex items-center justify-center gap-3 active:scale-95 duration-300 ease-out shadow-[0_0_24px_rgba(204,255,0,0.25)]">
<span class="material-symbols-outlined text-black font-bold" data-icon="bolt" style="font-variation-settings: 'FILL' 1;">bolt</span>
<span class="text-black font-headline font-black tracking-tight text-lg uppercase">START WORKOUT</span>
</button>
</div>
<!-- BottomNavBar -->
<nav class="fixed bottom-0 left-0 w-full flex justify-around items-center px-4 pb-6 pt-2 bg-[#0e0e0e]/80 backdrop-blur-2xl rounded-t-2xl z-50 no-border shadow-[0_-1px_0_0_rgba(204,255,0,0.05)] shadow-[0_-8px_32px_rgba(204,255,0,0.15)]">
<div class="flex flex-col items-center justify-center text-[#adaaaa] p-3 hover:text-[#CCFF00] transition-colors active:scale-90 duration-300 ease-out">
<span class="material-symbols-outlined" data-icon="home">home</span>
</div>
<div class="flex flex-col items-center justify-center bg-[#CCFF00] text-[#0e0e0e] rounded-xl p-3 shadow-[0_0_20px_rgba(204,255,0,0.4)] active:scale-90 duration-300 ease-out">
<span class="material-symbols-outlined" data-icon="fitness_center">fitness_center</span>
</div>
<div class="flex flex-col items-center justify-center text-[#adaaaa] p-3 hover:text-[#CCFF00] transition-colors active:scale-90 duration-300 ease-out">
<span class="material-symbols-outlined" data-icon="bolt">bolt</span>
</div>
<div class="flex flex-col items-center justify-center text-[#adaaaa] p-3 hover:text-[#CCFF00] transition-colors active:scale-90 duration-300 ease-out">
<span class="material-symbols-outlined" data-icon="leaderboard">leaderboard</span>
</div>
<div class="flex flex-col items-center justify-center text-[#adaaaa] p-3 hover:text-[#CCFF00] transition-colors active:scale-90 duration-300 ease-out">
<span class="material-symbols-outlined" data-icon="person">person</span>
</div>
</nav>
</body></html>