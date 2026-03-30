# AI Guidelines

Proyecto:Trimex (MAUI)
Stack: .NET MAUI, C#, SQLite local
Arquitectura: MVVM
Objetivo: App offline para configurar diferentes emporaizadores para entrenamientos y unos predefinidos que son los hero wods.

## AI Behavior

- Devuelve siempre código completo, no fragmentos
- No expliques salvo que se pida
- No inventes librerías
- Prioriza soluciones simples y mantenibles
- Respeta la estructura existente del proyecto

## Code Conventions

- C#: PascalCase en clases y métodos, camelCase en variables
- XAML separado de lógica (MVVM estricto)
- Un ViewModel por pantalla
- Comandos en lugar de eventos en UI

## Project Structure

- /Pages → XAML
- /Controls → control de UI
- /Models → datos
- /Services → acceso a datos / lógica externa

## Patterns

- MVVM obligatorio
- Inyección de dependencias para servicios

## Technical Rules

- Base de datos SQLite local
- Sin llamadas a APIs externas
- Persistencia siempre en servicios
- Manejo de errores con try/catch + logging


---

### 8. Qué NO hacer
```md
## Anti-Patterns

- No lógica en code-behind
- No mezclar UI y datos
- No usar variables globales