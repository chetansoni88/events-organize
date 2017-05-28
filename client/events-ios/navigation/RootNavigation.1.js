import React from 'react';
import { StyleSheet, View, StatusBar } from 'react-native';
import { LoginScreen } from '../screens/LoginScreen'
import {
  NavigationProvider,
  StackNavigation
} from '@expo/ex-navigation';

export default class RootNavigation extends React.Component {
  render() {
    return (
      
      <StackNavigation
        initialRoute='login'
      >
      </StackNavigation>
    );
  }
}
