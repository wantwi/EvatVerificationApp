import React, { Component } from 'react';
import SideBar from './SideBar';


export class Layout extends Component {
  static displayName = Layout.name;

  render() {
    return (
      <main className='min-h-screen flex'>
          <SideBar/>
        <>
          {this.props.children}
        </>

      
      </main>
    );
  }
}
