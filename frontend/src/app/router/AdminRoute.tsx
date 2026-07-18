import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../providers/AuthProvider";

const ADMIN_ROLE_ID = 1;

export function AdminRoute() {
  const { user } = useAuth();

  return user?.role_id === ADMIN_ROLE_ID ? (
    <Outlet />
  ) : (
    <Navigate to="/home/bandeja" replace />
  );
}