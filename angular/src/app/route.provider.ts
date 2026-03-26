import { RoutesService, eLayoutType } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

function configureRoutes(routesService: RoutesService) {
  return () => {
    routesService.add([
      {
        path: '/',
        name: '::Menu:Home',
        iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application,
      },
      {
        path: '/blogs',
        name: 'Blogs',
        iconClass: 'fas fa-blog',
        order: 2,
        layout: eLayoutType.application,
      },
      {
        path: '/blog-tags',
        name: '::Menu:BlogTags',
        parentName: '::Menu:Blogging',
        iconClass: 'fas fa-tags',
        order: 3,
        layout: eLayoutType.application,
        requiredPolicy: 'SaasDemo.BlogTag',
      },
      {
        path: '/media-library',
        name: 'مكتبة الوسائط',
        iconClass: 'fas fa-photo-video',
        order: 4,
        layout: eLayoutType.application,
        // requiredPolicy: 'SaasDemo.MediaLibrary', // Uncomment when ready
      }]);
  };
}
