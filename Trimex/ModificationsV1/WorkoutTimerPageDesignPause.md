<!DOCTYPE html>

<html class="dark" lang="en"><head>
<meta charset="utf-8"/>
<meta content="width=device-width, initial-scale=1.0" name="viewport"/>
<title>TRIMEX | AMRAP TIMER</title>
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
        .glow-shadow {
            box-shadow: 0 0 32px rgba(243, 255, 202, 0.15);
        }
    </style>
<style>
    body {
      min-height: max(884px, 100dvh);
    }
  </style>
  </head>
<body class="bg-background text-on-surface font-body selection:bg-primary-container selection:text-on-primary-container overflow-hidden">
<!-- Top Navigation Anchor -->
<header class="fixed top-0 w-full z-50 bg-surface/70 backdrop-blur-md dark:bg-black/70 flex items-center justify-between px-6 h-16 w-full">
<div class="flex items-center gap-4">
<button class="flex items-center justify-center w-10 h-10 rounded-full hover:bg-white/10 transition-colors active:scale-95 duration-200">
<span class="material-symbols-outlined text-on-surface">arrow_back</span>
</button>
<span class="text-xl font-bold italic text-[#CCFF00] font-['Space_Grotesk'] uppercase tracking-tighter">TRIMEX</span>
</div>
<div class="flex items-center gap-2">
<div class="px-3 py-1 bg-surface-container-highest rounded-full border border-outline-variant/20">
<span class="text-[10px] font-bold font-headline text-on-surface-variant tracking-widest uppercase">Live Session</span>
</div>
</div>
</header>
<main class="relative min-h-screen flex flex-col items-center justify-center px-6 pt-16">
<!-- Background Kinetic Element (Subtle) -->
<div class="absolute inset-0 z-0 pointer-events-none overflow-hidden">
<div class="absolute -top-24 -left-24 w-96 h-96 bg-primary/5 rounded-full blur-[100px]"></div>
<div class="absolute top-1/2 -right-24 w-64 h-64 bg-secondary/5 rounded-full blur-[80px]"></div>
</div>
<!-- Timer Content Layer -->
<div class="relative z-10 w-full max-w-md flex flex-col items-center">
<!-- Header Metrics -->
<div class="text-center mb-12">
<h1 class="font-headline font-bold text-6xl md:text-7xl tracking-tighter text-on-surface leading-none">AMRAP</h1>
<p class="font-label font-medium text-sm tracking-[0.3em] text-on-surface-variant mt-2">15:00 MINUTES</p>
</div>
<!-- Kinetic Pulse Circular Progress -->
<div class="relative w-80 h-80 flex items-center justify-center">
<!-- Outer Track (The Groove) -->
<svg class="absolute inset-0 w-full h-full -rotate-90">
<circle class="text-surface-container-highest" cx="160" cy="160" fill="transparent" r="140" stroke="currentColor" stroke-width="12"></circle>
<!-- Progress Bar (The Pulse) -->
<circle class="text-primary-container" cx="160" cy="160" fill="transparent" r="140" stroke="currentColor" stroke-dasharray="880" stroke-dashoffset="660" stroke-linecap="round" stroke-width="12"></circle>
</svg>
<!-- Center UI (Pause State) -->
<div class="relative z-20 flex flex-col items-center justify-center group cursor-pointer">
<div class="flex items-end gap-3 h-24 mb-4">
<!-- Custom Pause Icon: 3 vertical red lines -->
<div class="w-3 h-12 bg-error rounded-full opacity-80"></div>
<div class="w-3 h-20 bg-error rounded-full"></div>
<div class="w-3 h-12 bg-error rounded-full opacity-80"></div>
</div>
<div class="text-center">
<span class="block font-headline font-bold text-5xl text-on-surface">11:15</span>
<span class="block font-label text-[10px] tracking-[0.4em] text-on-surface-variant uppercase mt-1">Remaining</span>
</div>
</div>
<!-- Accent Glow -->
<div class="absolute inset-0 rounded-full glow-shadow pointer-events-none"></div>
</div>
<!-- Stats Bento Grid (Secondary Data) -->
<div class="grid grid-cols-2 gap-3 w-full mt-16">
<div class="bg-surface-container-low p-5 rounded-xl flex flex-col">
<span class="font-label text-[10px] tracking-widest text-on-surface-variant uppercase mb-1">Rounds Completed</span>
<span class="font-headline font-bold text-3xl text-secondary">08</span>
</div>
<div class="bg-surface-container-low p-5 rounded-xl flex flex-col">
<span class="font-label text-[10px] tracking-widest text-on-surface-variant uppercase mb-1">Heart Rate</span>
<div class="flex items-baseline gap-1">
<span class="font-headline font-bold text-3xl text-on-surface">164</span>
<span class="font-label text-xs text-on-surface-variant">BPM</span>
</div>
</div>
</div>
<!-- Control Layer (Slide Track) -->
<div class="w-full mt-12 mb-8">
<div class="relative h-20 bg-surface-container-highest rounded-xl p-2 flex items-center overflow-hidden">
<!-- Background Label -->
<div class="absolute inset-0 flex items-center justify-center pointer-events-none">
<span class="font-label font-bold text-[10px] tracking-[0.5em] text-on-surface-variant/30 uppercase">Slide to Reset</span>
</div>
<!-- The Track Track/Visual Groove -->
<div class="absolute left-2 right-2 h-1 bg-surface-container-low rounded-full"></div>
<!-- Reset Ball/Slider -->
<div class="relative z-10 w-16 h-16 kinetic-gradient rounded-lg flex items-center justify-center shadow-lg cursor-grab active:cursor-grabbing active:scale-95 transition-transform">
<span class="material-symbols-outlined text-on-primary-container text-3xl">refresh</span>
</div>
</div>
</div>
</div>
</main>
<!-- Contextual Footer Labels -->
<footer class="fixed bottom-8 w-full px-6 flex justify-between items-center z-50 pointer-events-none">
<div class="flex flex-col">
<span class="font-label text-[10px] tracking-widest text-on-surface-variant uppercase">Current Exercise</span>
<span class="font-headline font-medium text-lg text-on-surface">Burpee Box Overs</span>
</div>
<div class="flex flex-col items-end">
<span class="font-label text-[10px] tracking-widest text-on-surface-variant uppercase">Up Next</span>
<span class="font-headline font-medium text-lg text-on-surface-variant">Wall Balls</span>
</div>
</footer>
<!-- Global Decoration Elements -->
<div class="fixed bottom-0 left-0 w-full h-32 bg-gradient-to-t from-background to-transparent pointer-events-none"></div>
</body></html>