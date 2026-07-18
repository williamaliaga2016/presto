import React from 'react';

export const useDropdownTooltip = (labelField: string = 'description') => {
  
  // Template para las opciones de la lista desplegable
  const itemTemplate = (option: any) => {
    if (!option) return null;
    const text = option[labelField] ?? '';
    
    return (
      <div title={text} className="w-full truncate">
        {text}
      </div>
    );
  };

  // Template para la opción seleccionada (Dropdown cerrado)
  const valueTemplate = (option: any, props: any) => {
    if (!option) {
      return <span>{props.placeholder}</span>;
    }
    const text = option[labelField] ?? '';

    return (
      <div title={text} className="w-full truncate">
        {text}
      </div>
    );
  };

  return {
    itemTemplate,
    valueTemplate,
  };
};