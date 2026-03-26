<!DOCTYPE html>

<html class="dark" lang="en"><head>
<meta charset="utf-8"/>
<meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" name="viewport"/>
<title>TRIMEX EMOM Configuration</title>
<script src="https://cdn.tailwindcss.com?plugins=forms,container-queries"></script>
<link href="https://fonts.googleapis.com/css2?family=Space+Grotesk:wght@300;400;500;600;700;900&amp;family=Inter:wght@300;400;600;700;800&amp;display=swap" rel="stylesheet"/>
<link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:wght,FILL@100..700,0..1&amp;display=swap" rel="stylesheet"/>
<link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:wght,FILL@100..700,0..1&amp;display=swap" rel="stylesheet"/>
<script id="tailwind-config">
      tailwind.config = {
        darkMode: "class",
        theme: {
          extend: {
            colors: {
              "on-secondary": "#004d57",
              "secondary-dim": "#00d4ec",
              "on-secondary-fixed-variant": "#005964",
              "surface-variant": "#262626",
              "error-container": "#b92902",
              "surface-bright": "#2c2c2c",
              "primary-container": "#cafd00",
              "surface": "#0e0e0e",
              "secondary-fixed": "#26e6ff",
              "primary-fixed-dim": "#beee00",
              "on-secondary-fixed": "#003a42",
              "surface-tint": "#f3ffca",
              "primary-fixed": "#cafd00",
              "secondary-container": "#006875",
              "error-dim": "#d53d18",
              "on-error-container": "#ffd2c8",
              "on-tertiary-fixed": "#483d00",
              "on-error": "#450900",
              "tertiary-container": "#fce047",
              "on-primary-fixed": "#3a4a00",
              "outline-variant": "#484847",
              "tertiary": "#ffeea5",
              "on-surface": "#ffffff",
              "background": "#0e0e0e",
              "surface-container-low": "#131313",
              "on-background": "#ffffff",
              "surface-container-high": "#201f1f",
              "surface-dim": "#0e0e0e",
              "on-primary-container": "#4a5e00",
              "secondary-fixed-dim": "#00d7f0",
              "on-tertiary-container": "#5d5000",
              "inverse-primary": "#516700",
              "tertiary-fixed": "#fce047",
              "inverse-surface": "#fcf9f8",
              "tertiary-dim": "#edd13a",
              "on-primary": "#516700",
              "error": "#ff7351",
              "surface-container": "#1a1919",
              "surface-container-highest": "#262626",
              "on-surface-variant": "#adaaaa",
              "primary-dim": "#beee00",
              "on-tertiary": "#665800",
              "on-primary-fixed-variant": "#526900",
              "primary": "#f3ffca",
              "inverse-on-surface": "#565555",
              "outline": "#767575",
              "surface-container-lowest": "#000000",
              "on-tertiary-fixed-variant": "#685900",
              "on-secondary-container": "#e8fbff",
              "tertiary-fixed-dim": "#edd13a",
              "secondary": "#00e3fd"
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
      body { background-color: #0e0e0e; overflow: hidden; height: 100vh; }
      .kinetic-gradient { background: linear-gradient(135deg, #f3ffca 0%, #cafd00 100%); }
      .glass-shell { backdrop-filter: blur(20px); -webkit-backdrop-filter: blur(20px); }
      .material-symbols-outlined { font-variation-settings: 'FILL' 0, 'wght' 400, 'GRAD' 0, 'opsz' 24; }
    </style>
<style>
    body {
      min-height: max(884px, 100dvh);
    }
  </style>
  </head>
<body class="text-on-surface font-body selection:bg-primary-container selection:text-on-primary-container">
<!-- Top Navigation Shell -->
<header class="fixed top-0 w-full z-50 bg-[#0e0e0e]/70 backdrop-blur-md bg-gradient-to-b from-[#131313] to-transparent shadow-[0_0_20px_rgba(204,255,0,0.15)] flex justify-between items-center px-6 h-16 w-full">
<div class="flex items-center gap-4">
<span class="material-symbols-outlined text-[#CCFF00] active:scale-95 transition-transform" data-icon="menu">menu</span>
<h1 class="font-['Space_Grotesk'] font-bold uppercase tracking-tighter text-2xl font-black text-[#CCFF00] tracking-widest">EMOM</h1>
</div>
<div class="w-8 h-8 rounded-full bg-surface-container-highest overflow-hidden border border-outline-variant/15">
</div>
</header>
<!-- Main Content Canvas -->
<main class="pt-24 pb-32 px-6 max-w-lg mx-auto h-screen flex flex-col justify-center">
<!-- Header Text Block -->
<div class="mb-10 text-left">
<span class="text-secondary font-bold tracking-[0.2em] text-[10px] uppercase">Trimex Protocol</span>
<h2 class="font-headline text-6xl font-black tracking-tighter leading-none mt-2">EMOM</h2>
<p class="text-on-surface-variant font-medium tracking-tight text-lg mt-1 italic">Every Minute On the Minute</p>
</div>
<!-- Bento Configuration Grid -->
<div class="grid grid-cols-2 gap-4">
<!-- Duration Selector Card -->
<div class="bg-surface-container-low rounded-xl p-5 flex flex-col justify-between aspect-square border-b border-r border-outline-variant/5">
<span class="text-on-surface-variant font-bold text-[10px] tracking-[0.2em] uppercase">DURATION</span>
<div class="flex flex-col items-center justify-center flex-1">
<div class="text-4xl font-headline font-black text-primary tracking-tighter">01:00</div>
<div class="mt-4 flex items-center gap-1 text-[10px] text-secondary font-bold uppercase tracking-widest opacity-60">
<span class="material-symbols-outlined text-sm" data-icon="touch_app">touch_app</span>
                        Tap to edit
                    </div>
</div>
</div>
<!-- Rounds Selector Card -->
<div class="bg-surface-container-low rounded-xl p-5 flex flex-col justify-between aspect-square border-b border-l border-outline-variant/5">
<span class="text-on-surface-variant font-bold text-[10px] tracking-[0.2em] uppercase">ROUNDS</span>
<div class="flex flex-col items-center justify-center flex-1">
<div class="text-6xl font-headline font-black text-white tracking-tighter">10</div>
<div class="mt-4 flex items-center gap-1 text-[10px] text-secondary font-bold uppercase tracking-widest opacity-60">
<span class="material-symbols-outlined text-sm" data-icon="ads_click">ads_click</span>
                        Adjust
                    </div>
</div>
</div>
</div>
<!-- Summary Calculation Block -->
<div class="mt-8 flex items-end justify-between px-2">
<div>
<span class="block text-[10px] font-black text-on-surface-variant tracking-[0.3em] mb-1">SESSION VOLUME</span>
<div class="flex items-baseline gap-2">
<span class="text-3xl font-headline font-bold text-on-surface">TOTAL ROUNDS / 10 MIN</span>
</div>
</div>
<div class="w-12 h-[2px] bg-secondary mb-3"></div>
</div>
<!-- Action Section -->
<div class="mt-12 space-y-4">
<!-- Death by EMOM Button -->
<button class="w-full h-16 bg-surface-container-highest rounded-xl flex items-center justify-center gap-3 active:scale-[0.98] transition-all group">
<span class="material-symbols-outlined text-error" data-icon="skull">skull</span>
<span class="font-headline font-black text-error text-lg tracking-widest uppercase italic">DEATH BY EMOM</span>
</button>
<!-- Start Workout Button -->
<button class="w-full h-20 kinetic-gradient rounded-xl flex items-center justify-center gap-4 shadow-[0_0_30px_rgba(202,253,0,0.3)] active:scale-[0.98] transition-all">
<span class="material-symbols-outlined text-background text-3xl" data-icon="bolt" data-weight="fill" style="font-variation-settings: 'FILL' 1;">bolt</span>
<span class="font-headline font-black text-background text-2xl tracking-tighter uppercase">START WORKOUT</span>
</button>
</div>
</main>
<!-- Bottom Navigation Shell -->
<nav class="fixed bottom-0 w-full z-50 rounded-t-[1rem] bg-[#0e0e0e]/80 backdrop-blur-xl border-t border-[#484847]/15 shadow-[0_-8px_32px_rgba(0,0,0,0.5)] flex justify-around items-center px-4 py-3 pb-safe w-full">
<a class="flex flex-col items-center justify-center bg-[#CCFF00] text-[#0e0e0e] rounded-xl p-3 shadow-[0_0_15px_#CCFF00] active:scale-90 duration-150" href="#">
<span class="material-symbols-outlined" data-icon="timer">timer</span>
</a>
<a class="flex flex-col items-center justify-center text-[#adaaaa] p-3 hover:bg-[#202020] rounded-xl transition-all active:scale-90 duration-150" href="#">
<span class="material-symbols-outlined" data-icon="fitness_center">fitness_center</span>
</a>
<a class="flex flex-col items-center justify-center text-[#adaaaa] p-3 hover:bg-[#202020] rounded-xl transition-all active:scale-90 duration-150" href="#">
<span class="material-symbols-outlined" data-icon="explore">explore</span>
</a>
<a class="flex flex-col items-center justify-center text-[#adaaaa] p-3 hover:bg-[#202020] rounded-xl transition-all active:scale-90 duration-150" href="#">
<span class="material-symbols-outlined" data-icon="analytics">analytics</span>
</a>
<a class="flex flex-col items-center justify-center text-[#adaaaa] p-3 hover:bg-[#202020] rounded-xl transition-all active:scale-90 duration-150" href="#">
<span class="material-symbols-outlined" data-icon="person">person</span>
</a>
</nav>
<!-- Visual Texture Overlay -->
<div class="fixed inset-0 pointer-events-none opacity-[0.03] z-[60]" style="background-image: url('https://lh3.googleusercontent.com/aida-public/AB6AXuBj7nXyEwHm-gMQ0l5lKUo8OV2WvlB05T3AaweLRAg55wCWz8g1HekYXw3bZZbfiR0zjC2TJ8qP_D7jcmTqBPWnFfcLOtswriSOL5Op2xSILXIOZFwSS_4K2T3sEMCKImPfz9zsZNGaESvbID3y1kV-BrIAz11ThBIMjEcJHqW443GiHAxiYL-Cra2xuCthr6HTji-9QXibnFVALjAmmz9FDVUI-uHn4O0NtUmrVLBmCGJ6qFmXMxqDk0ukYydI87mwKf9yt8qXeJOH');">
</div>
</body></html>