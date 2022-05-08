export interface AfNode {
    type: NodeType,
    validators: AfValidator[]
}

export enum NodeType {
    Control = 1,
    Group = 2,
    Array = 3
}

export interface FormControlNode extends AfNode {
    type: NodeType.Control;
    value: any;
}

export interface FormGroupNode extends AfNode {
    type: NodeType.Group;
    nodes: { [key: string]: AfNode }
}

export interface FormArrayNode extends AfNode {
    type: NodeType.Array;
    nodes: AfNode[];
    nodeSchema: AfNode
}

export interface AfValidator {
    type: AfValidatorType
    value: any
}

export enum AfValidatorType {
    Required = 1,
    MinLength,
    MaxLength
}
