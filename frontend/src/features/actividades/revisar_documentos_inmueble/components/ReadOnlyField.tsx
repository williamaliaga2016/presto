type Props = {
  label: string;
  value?: string | number | boolean | null;
};

export default function ReadOnlyField({ label, value }: Props) {
  const display =
    value === null || value === undefined || value === ''
      ? '-'
      : typeof value === 'boolean'
        ? value
          ? 'Sí'
          : 'No'
        : String(value);

  return (
    <div className="flex flex-col gap-1">
      <label className="text-xs font-semibold uppercase tracking-wide text-gray-500">
        {label}
      </label>
      <p className="m-0 text-sm font-medium py-2 px-3 bg-gray-50 rounded border border-gray-200">
        {display}
      </p>
    </div>
  );
}