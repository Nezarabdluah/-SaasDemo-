import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SettingsRoutingModule } from './settings-routing.module';
import { SiteSettingsComponent } from './site-settings/site-settings.component';


@NgModule({
  imports: [
    CommonModule,
    SettingsRoutingModule,
    SiteSettingsComponent // Standalone component — imported, not declared
  ]
})
export class SettingsModule { }
