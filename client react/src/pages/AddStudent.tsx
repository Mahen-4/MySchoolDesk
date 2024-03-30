import React, {ChangeEvent } from "react"
import Axios from "axios";
function AddStudent() {

    const [selectedFile, setSelectedFile] = React.useState<File | null>(null);

    const handleClick = async() => {
        if (!selectedFile) return;

        try {
            const formData = new FormData();
            formData.append('file', selectedFile) 
            await Axios.post("http://localhost:5271/ManageStudent/upload", formData,{headers:{'Content-Type': 'multipart/form-data'}})
            .then(res => {
              console.log(res)
            })
        } catch (error) {
            console.log(error)
        }
    }
    const handleFileChange = (event: ChangeEvent<HTMLInputElement>) => {
        if (event.target.files && event.target.files.length > 0) {
            setSelectedFile(event.target.files[0]);
          }
      };
    return (
      <>
        <input type="file" onChange={handleFileChange}/>
        <button onClick={handleClick}>
            Create
        </button>
      </>
    )
  }
  
  export default AddStudent