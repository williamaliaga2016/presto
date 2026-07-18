import { Component, ReactNode } from 'react';

interface ErrorBoundaryProps {
  children: ReactNode;
}

interface ErrorBoundaryState {
  hasError: boolean;
}

export class ErrorBoundary extends Component<ErrorBoundaryProps, ErrorBoundaryState> {
  state: ErrorBoundaryState = { hasError: false };

  static getDerivedStateFromError(): ErrorBoundaryState {
    return { hasError: true };
  }

  componentDidCatch(error: Error, info: { componentStack: string }) {
    console.error(error, info.componentStack);
  }

  private handleReload = () => {
    window.location.reload();
  };

  render() {
    if (this.state.hasError) {
      return (
        <div className="flex min-h-screen flex-col items-center justify-center gap-4 p-6 text-center">
          <h1 className="text-xl font-semibold text-gray-800">
            Ocurrió un error inesperado
          </h1>
          <p className="max-w-md text-sm text-gray-600">
            Por favor recargue la página. Si el problema persiste, contacte a soporte.
          </p>
          <button
            type="button"
            onClick={this.handleReload}
            className="rounded-md bg-blue-600 px-4 py-2 text-sm font-medium text-white hover:bg-blue-700"
          >
            Recargar
          </button>
        </div>
      );
    }

    return this.props.children;
  }
}
