import React, { Component } from "react";

export class TodoItem extends Component {
  static displayName = TodoItem.name;

  constructor(props) {
    super(props);
    this.state = { todo: this.props.todo };
    this.toggleCompletion = this.toggleCompletion.bind(this);
  }

  toggleCompletion() {
    const todo = this.state.todo;
    this.props.onCompletionChange(todo.id);
  }

  render() {
    const todo = this.state.todo;
    let date = todo.dueDate ? (
      <small>{new Date(todo.dueDate).toLocaleString()}</small>
    ) : null;

    return (
      <div className="d-flex w-100 justify-content-between">
        <input
          className="form-check-input me-1"
          type="checkbox"
          checked={todo.completed}
          onChange={this.toggleCompletion}
        />
        <h5 className="mb-1">{todo.task}</h5>
        {date}
      </div>
    );
  }
}
