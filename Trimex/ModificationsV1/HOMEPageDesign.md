<!DOCTYPE html>

<html class="dark" lang="en"><head>
<meta charset="utf-8"/>
<meta content="width=device-width, initial-scale=1.0" name="viewport"/>
<title>TRIMEX | Kinetic Performance</title>
<script src="https://cdn.tailwindcss.com?plugins=forms,container-queries"></script>
<link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&amp;family=Space+Grotesk:wght@300;400;500;600;700;900&amp;display=swap" rel="stylesheet"/>
<link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:wght,FILL@100..700,0..1&amp;display=swap" rel="stylesheet"/>
<link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:wght,FILL@100..700,0..1&amp;display=swap" rel="stylesheet"/>
<script id="tailwind-config">
      tailwind.config = {
        darkMode: "class",
        theme: {
          extend: {
            colors: {
              "secondary": "#00e3fd",
              "error-dim": "#d53d18",
              "on-secondary-container": "#e8fbff",
              "primary-dim": "#beee00",
              "on-error-container": "#ffd2c8",
              "on-secondary-fixed": "#003a42",
              "surface-tint": "#f3ffca",
              "tertiary-dim": "#edd13a",
              "surface-container-low": "#131313",
              "tertiary": "#ffeea5",
              "primary-fixed-dim": "#beee00",
              "background": "#0e0e0e",
              "error": "#ff7351",
              "outline": "#767575",
              "inverse-on-surface": "#565555",
              "surface-container-high": "#201f1f",
              "on-primary-fixed-variant": "#526900",
              "secondary-container": "#006875",
              "on-surface": "#ffffff",
              "surface-container": "#1a1919",
              "on-secondary-fixed-variant": "#005964",
              "on-primary-fixed": "#3a4a00",
              "on-surface-variant": "#adaaaa",
              "on-tertiary-fixed-variant": "#685900",
              "secondary-fixed": "#26e6ff",
              "on-primary-container": "#4a5e00",
              "surface-dim": "#0e0e0e",
              "surface-container-highest": "#262626",
              "inverse-primary": "#516700",
              "tertiary-fixed": "#fce047",
              "surface": "#0e0e0e",
              "surface-variant": "#262626",
              "secondary-dim": "#00d4ec",
              "outline-variant": "#484847",
              "tertiary-container": "#fce047",
              "tertiary-fixed-dim": "#edd13a",
              "primary-fixed": "#cafd00",
              "primary": "#f3ffca",
              "on-secondary": "#004d57",
              "inverse-surface": "#fcf9f8",
              "secondary-fixed-dim": "#00d7f0",
              "on-background": "#ffffff",
              "primary-container": "#cafd00",
              "on-tertiary-fixed": "#483d00",
              "error-container": "#b92902",
              "surface-bright": "#2c2c2c",
              "surface-container-lowest": "#000000",
              "on-error": "#450900",
              "on-tertiary-container": "#5d5000",
              "on-tertiary": "#665800",
              "on-primary": "#516700"
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
        .hero-glow {
            box-shadow: 0 0 40px rgba(202, 253, 0, 0.3);
        }
    </style>
<style>
    body {
      min-height: max(884px, 100dvh);
    }
  </style>
  </head>
<body class="bg-background text-on-surface font-body selection:bg-primary selection:text-on-primary min-h-screen pb-24">
<!-- TopAppBar -->
<header class="fixed top-0 w-full z-50 bg-neutral-950/70 dark:bg-black/70 backdrop-blur-lg shadow-[0_4px_30px_rgba(0,0,0,0.5)]">
<div class="flex justify-between items-center px-6 h-16 w-full">
<div class="flex items-center gap-3">
<div class="w-10 h-10 rounded-full overflow-hidden border-2 border-[#CCFF00]/30 scale-95 active:scale-90 transition-transform">
</div>
<h1 class="text-2xl font-black italic text-[#CCFF00] tracking-widest font-['Space_Grotesk'] uppercase">TRIMEX</h1>
</div>
<div class="flex items-center gap-4">
<button class="material-symbols-outlined text-[#CCFF00] scale-95 active:scale-90 transition-transform">notifications</button>
</div>
</div>
</header>
<main class="pt-20 px-4 max-w-5xl mx-auto space-y-6">
<!-- Welcome Segment (Editorial Contrast) -->
<section class="mt-4 mb-2">
<p class="font-headline text-[0.75rem] font-bold tracking-[0.3em] text-on-surface-variant uppercase">SYSTEM READY</p>
<h2 class="font-headline text-4xl font-black tracking-tighter leading-none mt-1">CHOOSE YOUR <span class="text-primary-fixed">WEAPON</span></h2>
</section>
<!-- Training Modes Grid -->
<section class="grid grid-cols-2 gap-4">
<!-- Card 1: FOR TIME -->
<div class="aspect-square bg-surface-container-low rounded-xl p-5 flex flex-col justify-between group hover:bg-surface-container-high transition-all duration-300">
<div class="flex justify-between items-start">
<span class="material-symbols-outlined text-primary-fixed text-3xl">timer</span>
<span class="text-[0.6rem] font-bold text-on-surface-variant tracking-widest">01</span>
</div>
<div>
<h3 class="font-headline text-2xl font-black leading-tight uppercase tracking-tight">FOR<br/>TIME</h3>
<div class="h-1 w-8 bg-primary-fixed mt-2 rounded-full"></div>
</div>
</div>
<!-- Card 2: AMRAP -->
<div class="aspect-square bg-surface-container-low rounded-xl p-5 flex flex-col justify-between group hover:bg-surface-container-high transition-all duration-300">
<div class="flex justify-between items-start">
<span class="material-symbols-outlined text-secondary text-3xl">loop</span>
<span class="text-[0.6rem] font-bold text-on-surface-variant tracking-widest">02</span>
</div>
<div>
<h3 class="font-headline text-2xl font-black leading-tight uppercase tracking-tight">AMRAP</h3>
<p class="text-[0.65rem] text-on-surface-variant font-bold mt-1">AS MANY REPS AS POSSIBLE</p>
</div>
</div>
<!-- Card 3: EMOM -->
<div class="aspect-square bg-surface-container-low rounded-xl p-5 flex flex-col justify-between group hover:bg-surface-container-high transition-all duration-300">
<div class="flex justify-between items-start">
<span class="material-symbols-outlined text-tertiary text-3xl">schedule</span>
<span class="text-[0.6rem] font-bold text-on-surface-variant tracking-widest">03</span>
</div>
<div>
<h3 class="font-headline text-2xl font-black leading-tight uppercase tracking-tight">EMOM</h3>
<p class="text-[0.65rem] text-on-surface-variant font-bold mt-1">EVERY MINUTE ON THE MINUTE</p>
</div>
</div>
<!-- Card 4: TABATA -->
<div class="aspect-square bg-surface-container-low rounded-xl p-5 flex flex-col justify-between group hover:bg-surface-container-high transition-all duration-300">
<div class="flex justify-between items-start">
<span class="material-symbols-outlined text-error text-3xl">bolt</span>
<span class="text-[0.6rem] font-bold text-on-surface-variant tracking-widest">04</span>
</div>
<div>
<h3 class="font-headline text-2xl font-black leading-tight uppercase tracking-tight">TABATA</h3>
<div class="flex gap-1 mt-2">
<div class="h-1 w-4 bg-error rounded-full"></div>
<div class="h-1 w-2 bg-error/30 rounded-full"></div>
</div>
</div>
</div>
</section>
<!-- HERO WOD Section -->
<section class="relative overflow-hidden group cursor-pointer">
<div class="bg-[#CCFF00] rounded-xl p-6 flex flex-col md:flex-row items-center justify-between min-h-[160px] relative z-10 hero-glow overflow-hidden active:scale-95 transition-transform duration-150">
<!-- Abstract background texture -->
<div class="absolute inset-0 opacity-10 mix-blend-overlay pointer-events-none">
<img alt="Background texture" class="w-full h-full object-cover" data-alt="Abstract gritty texture with motion blur of heavy chains and weights in a dark gym environment" src="https://lh3.googleusercontent.com/aida-public/AB6AXuDXrkOnxE7NJSAXp4MseiRvNNME59jKfC20O6jv98pxFvxViZGFt6B0Qx29jQEfZYA87jjL__l0CfmNBw9FXieC1L1CwFHBM1HSY9wVg1WVM_rBM33jU10HkfLCoGWRsrQt1oXT61NR1p2e93-QNHP9Ti7iFNBuE0C2ma-FykJFvGzZdRSQLDCGO0O6LSPrHqZZELvAHyhU1aTyzP_ANnSyrK5ZJMHoz6J-yJqt1_mWtV638ZTeX1MB3DsvzNngnqcIzxtxFVvrwCu3"/>
</div>
<div class="flex flex-col relative z-20">
<span class="text-black font-headline font-black text-xs tracking-[0.4em] uppercase mb-1">ELITE CHALLENGE</span>
<h2 class="text-black font-headline text-5xl md:text-6xl font-black italic tracking-tighter leading-none uppercase">HERO WOD</h2>
</div>
<div class="mt-4 md:mt-0 flex items-center gap-6 relative z-20">
<div class="text-right">
</div>
<div class="bg-black text-[#CCFF00] w-14 h-14 rounded-full flex items-center justify-center shadow-2xl">
<span class="material-symbols-outlined text-3xl font-bold">play_arrow</span>
</div>
</div>
</div>
</section>
</main>

</body></html>