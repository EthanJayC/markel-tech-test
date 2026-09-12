import { HttpInterceptorFn } from '@angular/common/http';
import { environment } from '../environments/environment';

export const apiKeyInterceptor: HttpInterceptorFn = (req, next) => {
  return next(
    req.clone({
      setHeaders: {
        'X-Api-Key': environment.apiKey
      }
    })
  );
};
