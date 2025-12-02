import { 
  Select, 
  SelectContent, 
  SelectItem, 
  SelectTrigger, 
  SelectValue 
} from "@/components/ui/select";

type SelectValues = {
  id: string | number;
  name: string;
};

type SelectFieldProps = {
  classname?: string;
  data: SelectValues[];
  onChange: (value: string) => void;
  placeholder: string;
  value?: string;
  fieldSize?: string
};

export const SelectField = ({ data, placeholder, classname, onChange, value }: SelectFieldProps) => {
  return (
    <Select value={String(value)} onValueChange={onChange}>
      <SelectTrigger className={classname || "w-[180px]"}>
        <SelectValue placeholder={placeholder} />
      </SelectTrigger>
      <SelectContent>
        {data.map((x) => (
          <SelectItem key={x.id} value={String(x.id)}>
            {x.name}
          </SelectItem>
        ))}
      </SelectContent>
    </Select>
  );
};
