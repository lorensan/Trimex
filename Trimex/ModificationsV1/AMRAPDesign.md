<!DOCTYPE html>

<html class="dark" lang="en"><head>
<meta charset="utf-8"/>
<meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" name="viewport"/>
<title>AMRAP Configuration</title>
<script src="https://cdn.tailwindcss.com?plugins=forms,container-queries"></script>
<link href="https://fonts.googleapis.com/css2?family=Space+Grotesk:wght@300;400;500;600;700;900&amp;family=Inter:wght@400;700;900&amp;display=swap" rel="stylesheet"/>
<link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:wght,FILL@100..700,0..1&amp;display=swap" rel="stylesheet"/>
<link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:wght,FILL@100..700,0..1&amp;display=swap" rel="stylesheet"/>
<script id="tailwind-config">
      tailwind.config = {
        darkMode: "class",
        theme: {
          extend: {
            colors: {
              "background": "#0e0e0e",
              "on-secondary-container": "#e8fbff",
              "surface-container": "#1a1919",
              "secondary-dim": "#00d4ec",
              "primary-fixed-dim": "#beee00",
              "primary-dim": "#beee00",
              "on-tertiary-fixed": "#483d00",
              "surface-container-low": "#131313",
              "on-secondary-fixed-variant": "#005964",
              "surface-bright": "#2c2c2c",
              "surface-variant": "#262626",
              "tertiary-fixed-dim": "#edd13a",
              "tertiary-container": "#fce047",
              "surface-tint": "#f3ffca",
              "surface-container-high": "#201f1f",
              "primary-container": "#cafd00",
              "secondary": "#00e3fd",
              "on-secondary": "#004d57",
              "inverse-on-surface": "#565555",
              "tertiary-fixed": "#fce047",
              "primary": "#f3ffca",
              "on-surface-variant": "#adaaaa",
              "on-primary": "#516700",
              "surface-container-lowest": "#000000",
              "tertiary": "#ffeea5",
              "on-primary-fixed-variant": "#526900",
              "primary-fixed": "#cafd00",
              "on-primary-fixed": "#3a4a00",
              "outline": "#767575",
              "on-error-container": "#ffd2c8",
              "on-background": "#ffffff",
              "on-tertiary-container": "#5d5000",
              "error-container": "#b92902",
              "surface": "#0e0e0e",
              "on-tertiary": "#665800",
              "secondary-container": "#006875",
              "surface-dim": "#0e0e0e",
              "inverse-primary": "#516700",
              "secondary-fixed": "#26e6ff",
              "on-error": "#450900",
              "on-primary-container": "#4a5e00",
              "surface-container-highest": "#262626",
              "error": "#ff7351",
              "inverse-surface": "#fcf9f8",
              "on-tertiary-fixed-variant": "#685900",
              "error-dim": "#d53d18",
              "secondary-fixed-dim": "#00d7f0",
              "on-surface": "#ffffff",
              "on-secondary-fixed": "#003a42",
              "outline-variant": "#484847",
              "tertiary-dim": "#edd13a"
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
        body {
            background-color: #0e0e0e;
            color: #ffffff;
            font-family: 'Inter', sans-serif;
            -webkit-font-smoothing: antialiased;
            overflow: hidden;
        }
        .kinetic-gradient {
            background: linear-gradient(135deg, #f3ffca 0%, #cafd00 100%);
        }
        .glass-header {
            background: rgba(14, 14, 14, 0.7);
            backdrop-filter: blur(20px);
        }
        .material-symbols-outlined {
            font-variation-settings: 'FILL' 0, 'wght' 400, 'GRAD' 0, 'opsz' 24;
        }
        .glow-shadow {
            box-shadow: 0 0 32px rgba(202, 253, 0, 0.15);
        }
    </style>
<style>
    body {
      min-height: max(884px, 100dvh);
    }
  </style>
  </head>
<body class="bg-background text-on-background min-h-screen flex flex-col">
<!-- TopAppBar -->
<header class="fixed top-0 w-full z-50 flex items-center justify-between px-6 h-16 bg-[#0e0e0e]/70 backdrop-blur-xl">
<div class="flex items-center gap-4">
<span class="material-symbols-outlined text-[#CCFF00] active:scale-95 duration-200 cursor-pointer" data-icon="arrow_back">arrow_back</span>
<h1 class="text-[#CCFF00] font-['Space_Grotesk'] font-bold uppercase tracking-tighter text-xl">WORKOUT CONFIG</h1>
</div>
<div class="w-10 h-10 rounded-full overflow-hidden border border-outline-variant/20">
<img alt="Profile" class="w-full h-full object-cover" data-alt="Close up portrait of a determined professional athlete with sweat on skin, dramatic high-contrast gym lighting" src="https://lh3.googleusercontent.com/aida-public/AB6AXuD7gnojRjkCPoFZfaGw1jssWRWgA3M7EY58pqpJbmIH8cMxmhE6w442qjcrb3WzuW--6m3ZTK49_vvjDw0BiTxS9NXuuod8LlelYOmGXjEJz9j1xl-nesBD0J2rQ-0AI2675TMqIZyWpTrVY0EWXcZOtepC-45No8bXssbGMVaKfUlvB1fsMLV6gZ1udxzm6fO1HJq5m4e1nbSA9EMqDro1AxfoH6ulnYPJNvcQ5PqGxN2bSzNdZxwIZSmB6vu_ibb3ep3ZLX_hDIfg"/>
</div>
</header>
<!-- Main Canvas -->
<main class="flex-1 mt-16 mb-20 px-6 flex flex-col justify-center max-w-md mx-auto w-full">
<!-- Header Section -->
<div class="mb-8">
<h2 class="font-headline font-black text-5xl uppercase tracking-tighter leading-none mb-2">AMRAP</h2>
<p class="font-headline text-on-surface-variant text-sm tracking-tight uppercase">Select the number of minutes of your AMRAP</p>
</div>
<!-- Main Selector Card -->
<div class="relative group cursor-pointer active:scale-[0.98] transition-transform duration-200">
<div class="absolute -inset-0.5 bg-[#CCFF00]/20 rounded-xl blur opacity-30 group-hover:opacity-100 transition duration-500"></div>
<div class="relative bg-surface-container-low rounded-xl p-8 flex flex-col items-center justify-center overflow-hidden">
<div class="absolute top-0 right-0 p-4">
<span class="material-symbols-outlined text-on-surface-variant/30 text-4xl" data-icon="timer">timer</span>
</div>
<span class="font-label text-on-surface-variant text-[10px] font-black uppercase tracking-[0.2em] mb-4">MINUTES</span>
<div class="font-headline font-black text-8xl text-on-surface tracking-tighter tabular-nums flex items-baseline">
                    15<span class="text-primary text-4xl ml-1">:00</span>
</div>
<div class="mt-6 flex items-center gap-2 text-primary font-label text-[10px] font-black tracking-widest uppercase">
<span>Tap to edit</span>
<span class="material-symbols-outlined text-sm" data-icon="unfold_more">unfold_more</span>
</div>
</div>
</div>
<!-- Presets Grid -->
<div class="mt-8 grid grid-cols-4 gap-3">
<button class="bg-surface-container-high hover:bg-surface-container-highest transition-colors py-4 rounded-lg flex flex-col items-center justify-center active:scale-90 duration-150 border border-outline-variant/10">
<span class="font-headline font-bold text-lg">5:00</span>
</button>
<button class="bg-surface-container-high hover:bg-surface-container-highest transition-colors py-4 rounded-lg flex flex-col items-center justify-center active:scale-90 duration-150 border border-outline-variant/10">
<span class="font-headline font-bold text-lg">10:00</span>
</button>
<!-- Active State -->
<button class="bg-[#CCFF00] py-4 rounded-lg flex flex-col items-center justify-center active:scale-90 duration-150 shadow-[0_0_20px_rgba(204,255,0,0.3)]">
<span class="font-headline font-bold text-lg text-black">15:00</span>
</button>
<button class="bg-surface-container-high hover:bg-surface-container-highest transition-colors py-4 rounded-lg flex flex-col items-center justify-center active:scale-90 duration-150 border border-outline-variant/10">
<span class="font-headline font-bold text-lg">20:00</span>
</button>
</div>
<!-- Meta Data / Context -->
<div class="mt-10 bg-surface-container-highest/30 rounded-xl p-4 flex items-center gap-4">
<div class="h-10 w-1 bg-secondary rounded-full"></div>
<div>
<p class="font-label text-[10px] font-black text-on-surface-variant uppercase tracking-widest">Estimated Intensity</p>
<p class="font-headline font-bold text-on-surface text-lg">HIGH PERFORMANCE</p>
</div>
</div>
</main>
<!-- Bottom Action Area (CTA) -->
<div class="fixed bottom-24 w-full px-6 max-w-md left-1/2 -translate-x-1/2 z-40">
<button class="kinetic-gradient w-full py-5 rounded-xl flex items-center justify-center gap-3 active:scale-95 duration-200 glow-shadow group">
<span class="material-symbols-outlined text-black font-bold group-hover:rotate-12 transition-transform" data-icon="bolt" style="font-variation-settings: 'FILL' 1;">bolt</span>
<span class="font-headline font-black text-black text-lg uppercase tracking-tighter">START WORKOUT</span>
</button>
</div>
<!-- BottomNavBar -->
<nav class="fixed bottom-0 w-full rounded-t-[1rem] z-50 bg-[#0e0e0e]/80 backdrop-blur-2xl shadow-[0_-8px_32px_rgba(204,255,0,0.05)] flex justify-around items-center px-4 h-20 pb-safe">
<!-- TRAIN - ACTIVE -->
<a class="flex flex-col items-center justify-center text-[#CCFF00] bg-[#202020] rounded-[0.75rem] py-2 px-4 active:scale-90 duration-150 transition-all" href="#">
<span class="material-symbols-outlined mb-1" data-icon="fitness_center" style="font-variation-settings: 'FILL' 1;">fitness_center</span>
<span class="font-['Inter'] text-[10px] font-black uppercase tracking-widest">TRAIN</span>
</a>
<!-- STATS -->
<a class="flex flex-col items-center justify-center text-[#adaaaa] py-2 px-4 hover:bg-[#202020] rounded-[0.75rem] active:scale-90 duration-150 transition-all" href="#">
<span class="material-symbols-outlined mb-1" data-icon="monitoring">monitoring</span>
<span class="font-['Inter'] text-[10px] font-black uppercase tracking-widest">STATS</span>
</a>
<!-- PLAN -->
<a class="flex flex-col items-center justify-center text-[#adaaaa] py-2 px-4 hover:bg-[#202020] rounded-[0.75rem] active:scale-90 duration-150 transition-all" href="#">
<span class="material-symbols-outlined mb-1" data-icon="calendar_today">calendar_today</span>
<span class="font-['Inter'] text-[10px] font-black uppercase tracking-widest">PLAN</span>
</a>
<!-- PROFILE -->
<a class="flex flex-col items-center justify-center text-[#adaaaa] py-2 px-4 hover:bg-[#202020] rounded-[0.75rem] active:scale-90 duration-150 transition-all" href="#">
<span class="material-symbols-outlined mb-1" data-icon="person">person</span>
<span class="font-['Inter'] text-[10px] font-black uppercase tracking-widest">PROFILE</span>
</a>
</nav>
<!-- Visual Polish: Noise Texture Overlay -->
<div class="fixed inset-0 pointer-events-none opacity-[0.03] z-[100] mix-blend-overlay">
<svg viewbox="0 0 200 200" xmlns="http://www.w3.org/2000/svg">
<filter id="noise">
<feturbulence basefrequency="0.65" numoctaves="3" stitchtiles="stitch" type="fractalNoise"></feturbulence>
</filter>
<rect filter="url(#noise)" height="100%" width="100%"></rect>
</svg>
</div>
</body></html>