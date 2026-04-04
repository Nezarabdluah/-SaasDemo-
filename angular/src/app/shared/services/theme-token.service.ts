import { Injectable, Renderer2, RendererFactory2 } from '@angular/core';
import { SiteSettingsService } from './site-settings.service';

@Injectable({
  providedIn: 'root'
})
export class ThemeTokenService {
  private renderer: Renderer2;

  constructor(
    private siteSettingsService: SiteSettingsService,
    rendererFactory: RendererFactory2
  ) {
    this.renderer = rendererFactory.createRenderer(null, null);
  }

  applyThemeTokens(): void {
    this.siteSettingsService.get().subscribe({
      next: (settings) => {
        let cssString = '';

        if (settings.primaryColor) {
           const rgb = this.hexToRgb(settings.primaryColor);
           cssString += `
             /* --- 1. Variables Override --- */
             :root, body, [data-theme], .lpx-theme-dark, .lpx-theme-light {
               --lpx-brand: ${settings.primaryColor} !important;
               --bs-primary: ${settings.primaryColor} !important;
               --bs-primary-rgb: ${rgb} !important;
               --lpx-primary: ${settings.primaryColor} !important;
             }

             /* --- 2. Deep Bootstrap Overrides --- */
             .btn-primary {
               background-color: ${settings.primaryColor} !important;
               border-color: ${settings.primaryColor} !important;
               color: #ffffff !important;
             }
             .btn-primary:active, .btn-primary:focus, .btn-primary:hover {
               filter: brightness(0.85) !important;
             }
             .btn-outline-primary {
               color: ${settings.primaryColor} !important;
               border-color: ${settings.primaryColor} !important;
             }
             .btn-outline-primary:hover {
               background-color: ${settings.primaryColor} !important;
               color: #ffffff !important;
             }
             .text-primary { color: ${settings.primaryColor} !important; }
             .bg-primary { background-color: ${settings.primaryColor} !important; color: #ffffff !important; }
             
             /* --- 3. Deep Lepton-X Overrides --- */
             .lpx-brand-name { color: ${settings.primaryColor} !important; }
             .lpx-nav-menu .lpx-menu-item-link.selected {
               background-color: rgba(${rgb}, 0.1) !important;
               color: ${settings.primaryColor} !important;
               border-left-color: ${settings.primaryColor} !important;
             }
             .lpx-nav-menu .lpx-menu-item-link:hover {
               color: ${settings.primaryColor} !important;
             }
             .page-header .page-title {
               border-left: 4px solid ${settings.primaryColor};
               padding-inline-start: 10px;
             }
           `;
        }

        if (settings.secondaryColor) {
           const rgb = this.hexToRgb(settings.secondaryColor);
           cssString += `
             :root, body, [data-theme], .lpx-theme-dark, .lpx-theme-light {
               --bs-secondary: ${settings.secondaryColor} !important;
               --bs-secondary-rgb: ${rgb} !important;
               --lpx-secondary: ${settings.secondaryColor} !important;
             }
             
             .btn-secondary {
               background-color: ${settings.secondaryColor} !important;
               border-color: ${settings.secondaryColor} !important;
               color: #ffffff !important;
             }
             .btn-secondary:active, .btn-secondary:focus, .btn-secondary:hover {
               filter: brightness(0.85) !important;
             }
             .text-secondary { color: ${settings.secondaryColor} !important; }
             .bg-secondary { background-color: ${settings.secondaryColor} !important; color: #ffffff !important; }
           `;
        }

        if (cssString) {
          // Remove old dynamic styles if they exist (for live updates without reload)
          const oldStyle = document.getElementById('dynamic-site-theme');
          if (oldStyle) {
            this.renderer.removeChild(document.head, oldStyle);
          }

          const style = this.renderer.createElement('style');
          this.renderer.setAttribute(style, 'id', 'dynamic-site-theme');
          this.renderer.appendChild(style, this.renderer.createText(cssString));
          this.renderer.appendChild(document.head, style);
        }
      },
      error: (err) => {
        console.error('Failed to load theme tokens:', err);
      }
    });
  }

  // Helper to convert HEX to RGB for Bootstrap variables that need it
  private hexToRgb(hex: string): string {
    const result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
    return result ? 
      `${parseInt(result[1], 16)}, ${parseInt(result[2], 16)}, ${parseInt(result[3], 16)}` 
      : '0, 0, 0';
  }
}
