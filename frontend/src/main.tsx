import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import { AppProviders } from "./app/providers/AppProviders";
import { ErrorBoundary } from "./shared/components/ErrorBoundary";

import "primereact/resources/themes/lara-light-blue/theme.css";
import "primereact/resources/primereact.min.css";
import "primeicons/primeicons.css";
import "./index.css";

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <ErrorBoundary>
      <AppProviders>
        <App />
      </AppProviders>
    </ErrorBoundary>
  </React.StrictMode>
);