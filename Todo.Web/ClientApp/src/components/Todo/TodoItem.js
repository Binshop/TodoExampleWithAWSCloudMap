import React, { Component } from "react";

export class TodoItem extends Component {
    constructor(props) {
        super(props);
        this.state = { todo: this.props.todo };
        this.toggleCompletion = this.toggleCompletion.bind(this);
    }

    toggleCompletion() {
        const todo = this.state.todo;
        console.log(
            `Change the completion status: ${
                todo.completed
            } -> ${!todo.completed}`
        );
        this.props.onCompletionChange(todo.id);
    }

    render() {
        const todo = this.state.todo;
        return (
            <div className="d-flex w-100 justify-content-between">
                <input
                    className="form-check-input me-1"
                    type="checkbox"
                    checked={todo.completed}
                    onChange={this.toggleCompletion}
                />
                <h5 className="mb-1">{todo.task}</h5>
                <small>{TodoItem.formatDate(todo.dueDate)}</small>
            </div>
        );
    }

    static formatDate(date) {
        let formatedDate = "";
        if (date) {
            const newDate = new Date(date);
            formatedDate = newDate.toLocaleString();
        }
        return formatedDate;
    }
}
