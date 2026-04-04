import { Component, OnInit } from '@angular/core';
import { InternetConnectionStatusComponent, LoaderBarComponent } from '@abp/ng.theme.shared';
import { DynamicLayoutComponent } from '@abp/ng.core';
import { ReplaceableComponentsService } from '@abp/ng.core';
import { eThemeLeptonXComponents } from '@abp/ng.theme.lepton-x';
import { DynamicFooterComponent } from './shared/components/dynamic-footer/dynamic-footer.component';
import { ThemeTokenService } from './shared/services/theme-token.service';

@Component({
  selector: 'app-root',
  template: `
    <abp-loader-bar />
    <abp-dynamic-layout />
    <abp-internet-status />
  `,
  imports: [LoaderBarComponent, DynamicLayoutComponent, InternetConnectionStatusComponent],
})
export class AppComponent implements OnInit {
  constructor(
    private replaceableComponents: ReplaceableComponentsService,
    private themeTokenService: ThemeTokenService
  ) {
    this.replaceableComponents.add({
      component: DynamicFooterComponent,
      key: eThemeLeptonXComponents.Footer,
    });
  }

  ngOnInit() {
    this.themeTokenService.applyThemeTokens();
  }
}
