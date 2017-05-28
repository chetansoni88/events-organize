import React, { Component } from 'react';
import {
    Image,
    Linking,
    Platform,
    ScrollView,
    StyleSheet,
    View,
} from 'react-native';
import DatePicker from 'react-native-datepicker'

export class DatetimePicker extends React.Component {
  constructor(props){
    super(props)
    this.state = {datetime: '2016-05-05 20:00'}
  }

  render(){
    return (
     <DatePicker
          style={{width: 200}}
          date={this.state.datetime}
          mode="datetime"
          format="YYYY-MM-DD HH:mm"
          confirmBtnText="Confirm"
          cancelBtnText="Cancel"
          customStyles={{
            dateIcon: {
              position: 'absolute',
              left: 200,
              top: 4,
              marginLeft: 0
            },
            dateInput: {
              marginLeft: 36
            }
          }}
          minuteInterval={10}
          onDateChange={(datetime) => {this.setState({datetime: datetime});}}
        />
    )
  }
}