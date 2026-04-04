# Goal
Construir una landing page moderna, visual y no convencional para el proyecto existente.

- Enfocada en impacto visual y claridad rápida del producto
- Navegación no tradicional (sin navbar superior clásica)
- Experiencia fluida tipo app
- Componentes reutilizables en React
- Preparada para escalar (añadir futuras secciones sin rehacer estructura)

Secciones requeridas:
- Home
- About
- How It Works
- Contact

Elementos clave:
- Menú lateral fijo (no superior)
- Botones redondos centrados en pantalla como navegación principal
- Header visual atractivo (hero section potente)


# Implementation

Arquitectura general:
- SPA en React (preferible con Vite o Next.js si se quiere SSR)
- Estructura por componentes desacoplados
- Navegación por scroll + estados (o React Router si se prefiere separación real)

Layout principal:
- Sidebar lateral fijo (izquierda o derecha)
  - Íconos minimalistas
  - Hover con labels
- Zona central dinámica
- Header tipo hero ocupando viewport inicial

Home:
- Hero section:
  - Título claro + subtítulo
  - Call to action principal
  - Fondo visual (gradiente, animación ligera o canvas)
- Botones redondos centrados:
  - 3–4 acciones principales
  - Animaciones hover (scale + glow)
  - Navegan a secciones

About:
- Explicación directa del producto
- Bloques cortos (no texto largo)
- Iconos + bullets

How It Works:
- Flujo en pasos (1 → 2 → 3)
- Visual tipo timeline o cards horizontales
- Animaciones al hacer scroll

Contact:
- Formulario simple:
  - Nombre
  - Email
  - Mensaje
- Feedback visual al enviar
- Alternativa: enlaces directos (email, github, etc.)

Menú lateral:
- Botones circulares
- Icon-only (tooltip en hover)
- Estado activo marcado
- Scroll suave a secciones

Header:
- Ocupa 100vh
- Tipografía grande
- Fondo atractivo:
  - Gradiente dinámico o animación ligera
- CTA visible sin hacer scroll


# Technical Details

Stack:
- React
- CSS: Tailwind (recomendado) o CSS Modules
- Animaciones: Framer Motion
- Routing opcional: React Router

Estructura:
- /components
  - Header.jsx
  - Sidebar.jsx
  - RoundMenu.jsx
  - Section.jsx
- /pages o /sections
  - Home.jsx
  - About.jsx
  - HowItWorks.jsx
  - Contact.jsx

Patrones:
- Componentes pequeños y reutilizables
- Props claras, sin lógica mezclada
- Separar UI de lógica

Estilos:
- Diseño limpio, minimalista
- Uso de:
  - Bordes redondos
  - Sombras suaves
  - Espaciado amplio
- Paleta consistente (definir 2–3 colores principales)

Animaciones:
- Hover en botones (scale + opacity)
- Entrada en scroll (fade + translate)
- Transiciones suaves entre secciones

Responsive:
- Mobile-first
- Sidebar → bottom nav o floating buttons en móvil
- Botones redondos adaptados a touch

Performance:
- Lazy load de secciones
- Evitar renders innecesarios
- Optimizar assets visuales

Extras opcionales:
- Modo oscuro
- Microinteracciones (hover, click feedback)
- Scroll snapping entre secciones
- Scroll snapping entre secciones