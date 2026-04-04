import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SiteSettingsComponent } from './site-settings/site-settings.component';
import { authGuard } from '@abp/ng.core';

const routes: Routes = [
  { path: '', component: SiteSettingsComponent, canActivate: [authGuard] }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SettingsRoutingModule { }
