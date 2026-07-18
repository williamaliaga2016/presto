export interface CatalogoOption {
  code?: string | null;
  description?: string | null;
}


export interface ControlesGenerarBorradorEscritura {
  notaria: CatalogoOption[];
  rolcomparecencia : CatalogoOption[];
}
