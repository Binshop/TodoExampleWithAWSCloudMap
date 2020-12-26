import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layouts/Layout';
import { Home } from './components/Home';
import { TodoList } from './components/Todo/TodoList';
import { Counter } from './components/Counter';

import './style.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <Route path='/todos' component={TodoList} />
      </Layout>
    );
  }
}
