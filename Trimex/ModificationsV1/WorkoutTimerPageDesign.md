<!DOCTYPE html>

<html class="dark" lang="en"><head>
<meta charset="utf-8"/>
<meta content="width=device-width, initial-scale=1.0" name="viewport"/>
<title>TRIMEX - AMRAP Workout Timer</title>
<script src="https://cdn.tailwindcss.com?plugins=forms,container-queries"></script>
<link href="https://fonts.googleapis.com/css2?family=Space+Grotesk:wght@300;400;500;600;700&amp;family=Inter:wght@300;400;500;600;700&amp;display=swap" rel="stylesheet"/>
<link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:wght,FILL@100..700,0..1&amp;display=swap" rel="stylesheet"/>
<link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:wght,FILL@100..700,0..1&amp;display=swap" rel="stylesheet"/>
<script id="tailwind-config">
        tailwind.config = {
            darkMode: "class",
            theme: {
                extend: {
                    colors: {
                        "on-surface": "#ffffff",
                        "primary-container": "#cafd00",
                        "tertiary-fixed": "#fce047",
                        "on-primary-fixed": "#3a4a00",
                        "on-primary-container": "#4a5e00",
                        "inverse-primary": "#516700",
                        "secondary-fixed": "#26e6ff",
                        "on-error-container": "#ffd2c8",
                        "surface-dim": "#0e0e0e",
                        "secondary-container": "#006875",
                        "on-secondary": "#004d57",
                        "error": "#ff7351",
                        "primary": "#f3ffca",
                        "background": "#0e0e0e",
                        "tertiary-container": "#fce047",
                        "surface-container-high": "#201f1f",
                        "tertiary-dim": "#edd13a",
                        "surface-tint": "#f3ffca",
                        "on-secondary-fixed": "#003a42",
                        "surface-variant": "#262626",
                        "outline": "#767575",
                        "on-tertiary-fixed-variant": "#685900",
                        "primary-fixed": "#cafd00",
                        "secondary-dim": "#00d4ec",
                        "outline-variant": "#484847",
                        "surface-bright": "#2c2c2c",
                        "on-error": "#450900",
                        "surface-container-highest": "#262626",
                        "surface": "#0e0e0e",
                        "on-tertiary-container": "#5d5000",
                        "on-secondary-fixed-variant": "#005964",
                        "primary-dim": "#beee00",
                        "error-container": "#b92902",
                        "on-primary": "#516700",
                        "secondary": "#00e3fd",
                        "primary-fixed-dim": "#beee00",
                        "tertiary-fixed-dim": "#edd13a",
                        "on-primary-fixed-variant": "#526900",
                        "on-background": "#ffffff",
                        "surface-container-low": "#131313",
                        "inverse-surface": "#fcf9f8",
                        "surface-container-lowest": "#000000",
                        "on-surface-variant": "#adaaaa",
                        "secondary-fixed-dim": "#00d7f0",
                        "inverse-on-surface": "#565555",
                        "on-tertiary-fixed": "#483d00",
                        "on-secondary-container": "#e8fbff",
                        "tertiary": "#ffeea5",
                        "surface-container": "#1a1919",
                        "on-tertiary": "#665800",
                        "error-dim": "#d53d18"
                    },
                    fontFamily: {
                        "headline": ["Space Grotesk"],
                        "body": ["Inter"],
                        "label": ["Inter"]
                    },
                    borderRadius: { "DEFAULT": "0.25rem", "lg": "0.5rem", "xl": "0.75rem", "full": "9999px" },
                },
            },
        }
    </script>
<style>
        .material-symbols-outlined {
            font-variation-settings: 'FILL' 0, 'wght' 400, 'GRAD' 0, 'opsz' 24;
        }
        .kinetic-pulse-glow {
            box-shadow: 0 0 32px 4px rgba(202, 253, 0, 0.15);
        }
        .play-button-glow {
            filter: drop-shadow(0 0 12px rgba(202, 253, 0, 0.6));
        }
        .progress-ring-circle {
            transition: stroke-dashoffset 0.35s;
            transform: rotate(-90deg);
            transform-origin: 50% 50%;
        }
        body {
            background-color: #0e0e0e;
            color: #ffffff;
            font-family: 'Inter', sans-serif;
        }
    </style>
<style>
    body {
      min-height: max(884px, 100dvh);
    }
  </style>
  </head>
<body class="bg-background selection:bg-primary-container selection:text-on-primary-container overflow-hidden">
<!-- Top Navigation Bar -->
<header class="fixed top-0 w-full z-50 bg-surface/70 backdrop-blur-md flex items-center justify-between px-6 h-16 w-full">
<div class="flex items-center gap-4">
<button class="flex items-center justify-center w-10 h-10 rounded-full hover:bg-white/10 active:scale-95 duration-200 transition-colors">
<span class="material-symbols-outlined text-on-surface opacity-60">arrow_back</span>
</button>
</div>
<div class="absolute left-1/2 -translate-x-1/2">
<h1 class="font-['Space_Grotesk'] uppercase tracking-tighter text-xl font-bold italic text-[#CCFF00]">TRIMEX</h1>
</div>
<div class="flex items-center gap-4">
<button class="flex items-center justify-center w-10 h-10 rounded-full hover:bg-white/10 active:scale-95 duration-200 transition-colors">
<span class="material-symbols-outlined text-on-surface opacity-60">settings</span>
</button>
</div>
</header>
<main class="relative min-h-screen flex flex-col items-center justify-between py-24 px-6 overflow-hidden">
<!-- Header Text Section -->
<div class="w-full flex flex-col items-center gap-1 z-10">
<h2 class="font-headline text-5xl md:text-7xl font-bold tracking-tighter uppercase text-on-surface">AMRAP</h2>
<div class="flex items-center gap-3">
<div class="h-[1px] w-8 bg-primary-container/30"></div>
<p class="font-headline text-lg md:text-xl font-medium tracking-[0.2em] text-on-surface-variant uppercase">15:00 MINUTES</p>
<div class="h-[1px] w-8 bg-primary-container/30"></div>
</div>
</div>
<!-- Central Timer Section -->
<div class="relative flex items-center justify-center w-full max-w-md aspect-square">
<!-- Background Ring (Track) -->
<svg class="absolute w-full h-full p-4" viewbox="0 0 100 100">
<circle class="text-surface-container-highest" cx="50" cy="50" fill="transparent" r="46" stroke="currentColor" stroke-width="2"></circle>
<!-- Neon Track (Simulated Progress) -->
<circle class="text-primary-container progress-ring-circle" cx="50" cy="50" fill="transparent" r="46" stroke="currentColor" stroke-dasharray="289.027" stroke-dashoffset="72.25" stroke-linecap="round" stroke-width="2.5"></circle>
</svg>
<!-- Inner Play Button -->
<button class="group relative z-20 flex items-center justify-center w-48 h-48 rounded-full bg-surface-container-high kinetic-pulse-glow active:scale-90 transition-all duration-300">
<div class="absolute inset-0 rounded-full border border-primary-container/20 group-hover:border-primary-container/40 transition-colors"></div>
<div class="play-button-glow translate-x-1">
<span class="material-symbols-outlined text-[100px] text-primary-container" style="font-variation-settings: 'FILL' 1;">play_arrow</span>
</div>
</button>
<!-- Radial Background Glow (Decorative) -->
<div class="absolute w-[120%] h-[120%] bg-primary-container/5 rounded-full blur-[100px] pointer-events-none"></div>
</div>
<!-- Metrics Grid (Asymmetric) -->
<div class="w-full max-w-lg grid grid-cols-2 gap-4 z-10">
<div class="bg-surface-container-low p-5 rounded-xl flex flex-col gap-1">
<span class="font-label text-[10px] tracking-widest text-on-surface-variant uppercase">HEART RATE</span>
<div class="flex items-baseline gap-1">
<span class="font-headline text-3xl font-bold text-secondary">142</span>
<span class="font-label text-xs text-secondary-dim">BPM</span>
</div>
</div>
<div class="bg-surface-container-low p-5 rounded-xl flex flex-col gap-1">
<span class="font-label text-[10px] tracking-widest text-on-surface-variant uppercase">ROUNDS</span>
<div class="flex items-baseline gap-1">
<span class="font-headline text-3xl font-bold text-on-surface">04</span>
<span class="font-label text-xs text-on-surface-variant">TOTAL</span>
</div>
</div>
</div>
<!-- Footer Action: Slide to Reset -->
<div class="w-full max-w-sm px-4 pb-8 z-10">
<div class="relative h-16 w-full bg-surface-container-highest rounded-full flex items-center p-1.5 cursor-pointer group">
<div class="absolute inset-0 flex items-center justify-center pointer-events-none">
<span class="font-label text-xs font-bold tracking-[0.25em] text-on-surface-variant group-hover:text-on-surface transition-colors uppercase">SLIDE TO RESET</span>
</div>
<!-- Slider Ball -->
<div class="h-12 w-12 rounded-full bg-primary-container flex items-center justify-center shadow-lg active:scale-95 transition-transform">
<span class="material-symbols-outlined text-on-primary-container">double_arrow</span>
</div>
</div>
</div>
<!-- Background Elements (Kinetic Aesthetic) -->
<div class="absolute top-0 right-0 p-8 opacity-10 pointer-events-none">
<span class="font-headline text-[15rem] leading-none font-extrabold tracking-tighter select-none">15</span>
</div>
<div class="absolute bottom-0 left-0 p-8 opacity-10 pointer-events-none">
<span class="font-headline text-[10rem] leading-none font-extrabold tracking-tighter select-none">MIN</span>
</div>
</main>
<!-- Overlay Textures -->
<div class="fixed inset-0 pointer-events-none opacity-[0.03] bg-[url('https://www.transparenttextures.com/patterns/stardust.png')]"></div>
</body></html>