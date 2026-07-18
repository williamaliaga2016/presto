import jquery from 'jquery';
import React from 'react';
import { createRoot } from 'react-dom/client';
import createReactClass from 'create-react-class';

declare global {
  interface Window {
    $: JQueryStatic;
    jQuery: JQueryStatic;
    React: typeof React;
    ReactDOM: unknown;
    createReactClass: typeof createReactClass;
  }
}

window.$ = window.jQuery = jquery;
window.React = React;
window.ReactDOM = { createRoot };
window.createReactClass = createReactClass;

// Parche necesario para compatibilidad con algunos scripts legacy de Bold Reports.
// jQuery 3 mantiene internamente esta lógica, pero no siempre expone $.isWindow.
if (typeof window.jQuery.isWindow !== 'function') {
  window.jQuery.isWindow = function (obj: unknown): boolean {
    return obj != null && obj === (obj as Window).window;
  } as JQueryStatic['isWindow'];
}
