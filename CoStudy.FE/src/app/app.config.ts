import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter, withPreloading, PreloadAllModules } from '@angular/router';
import { provideHttpClient, withInterceptors, withFetch } from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { SocialAuthServiceConfig, GoogleLoginProvider, SOCIAL_AUTH_CONFIG } from '@abacritt/angularx-social-login';
import { environment } from '../environments/environment';
import { tokenInterceptor } from './core/interceptors/token.interceptor';

import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes, withPreloading(PreloadAllModules)),
    provideHttpClient(withFetch(), withInterceptors([tokenInterceptor])),
    provideAnimationsAsync(),
    {
      provide: SOCIAL_AUTH_CONFIG,
      useValue: {
        autoLogin: false,
        providers: [
          // {
          //   id: GoogleLoginProvider.PROVIDER_ID,
          //   provider: new GoogleLoginProvider(
          //     environment.googleClientId
          //   )
          // }
        ],
        onError: (err) => {
          console.error(err);
        }
      } as SocialAuthServiceConfig,
    }
  ]
};
