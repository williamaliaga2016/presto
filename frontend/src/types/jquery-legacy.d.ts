import 'jquery';

declare global {
  interface JQueryStatic {
    isWindow?: (obj: unknown) => boolean;
  }
}

export {};
