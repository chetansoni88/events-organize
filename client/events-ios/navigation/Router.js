import { createRouter } from '@expo/ex-navigation';

import LoginScreen from '../screens/LoginScreen';
import HomeTabNavigation from './HomeTabNavigation';
import RootNavigation from './RootNavigation.1';
import Events from '../screens/Events';
import Profile from '../screens/Profile';


export default createRouter(() => ({
  rootNavigation: () => RootNavigation,
  login: () => LoginScreen,
  tabNavigation: () => HomeTabNavigation,
  profile: () => Profile,
  events: () => Events,
}));
